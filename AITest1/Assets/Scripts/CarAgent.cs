using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using System.Linq;
using Unity.Sentis;

public class CarAgent : Agent
{
    public List<Transform> targets;
    public Transform[] carSensors;
    public CarControl carControl;
    public LayerMask obstacleLayer;

    Rigidbody rBody;
    Transform target;
    Transform behindTarget;
    int targetIndex = -1;
    bool reachedTarget;
    int stuckStepCount;
    int hesitateStepCount;
    Vector3 previousPosition;
    Vector3 initPosition;

    const float rayDistance = 2.0f;
    const float fallY = -0.1f;
    const float sensitivity = 0.2f;
    const float targetReachDistance = 7f;

    public override void Initialize()
    {
        base.Initialize();
        rBody = GetComponent<Rigidbody>();
        NextTarget();
    }

    public override void OnEpisodeBegin()
    {
        if (!reachedTarget && hesitateStepCount <= 2000)
        {
            var previousTarget = GetPreviousTarget();
            this.transform.position = previousTarget.position;
            this.transform.rotation = previousTarget.rotation;
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        }

        reachedTarget = false;
        stuckStepCount = 0;
        hesitateStepCount = 0;
        initPosition = transform.localPosition;
        previousPosition = transform.localPosition;
        behindTarget = GetBehindTarget();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Init position
        sensor.AddObservation(initPosition);

        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity);

        // Car sensors
        for (int i = 0; i < carSensors.Length; i++)
        {
            sensor.AddObservation(CheckRay(carSensors[i]));
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        var vInput = actionBuffers.ContinuousActions[0];
        var hInput = actionBuffers.ContinuousActions[1];
        carControl.UpdateCar(vInput, hInput);

        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        float distanceToBehindTarget = Vector3.Distance(transform.localPosition, behindTarget.localPosition);

        if (target.localPosition.y > (transform.localPosition.y + 0.1f))
        {
            if (vInput < 0)
            {
                SetCarReward(-1.0f);
                EndEpisode();
                return;
            }
        }

        if (distanceToBehindTarget <= targetReachDistance)
        {
            SetCarReward(-1.0f);
            EndEpisode();
            return;
        }

        if (distanceToTarget <= targetReachDistance)
        {
            reachedTarget = true;
            SetCarReward(1.0f);
            NextTarget();
            EndEpisode();
        }
        // Fell off platform
        else if (this.transform.localPosition.y <= fallY)
        {
            SetCarReward(-1f);
            EndEpisode();
        }
        else
        {
            // Car sensors
            for (int i = 0; i < carSensors.Length; i++)
            {
                var checkRay = CheckRay(carSensors[i]);

                if (checkRay < sensitivity)
                {
                    SetCarReward(-0.5f);
                    EndEpisode();
                    break;
                }
            }
        }

        if (Vector3.Distance(previousPosition, transform.localPosition) < 0.01f)
        {
            stuckStepCount++;

            if (stuckStepCount > 200)
            {
                SetCarReward(-0.5f);
                EpisodeInterrupted();
                return;
            }
        }
        else
        {
            stuckStepCount = 0;
        }

        if (Vector3.Distance(initPosition, transform.localPosition) < targetReachDistance)
        {
            hesitateStepCount++;

            if (hesitateStepCount > 2000)
            {
                SetCarReward(-0.5f);
                EndEpisode();
                return;
            }
        }
        else
        {
            hesitateStepCount = 0;
        }

        previousPosition = transform.localPosition;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("RightStickY");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    private void NextTarget()
    {
        if (target != null)
        {
            target.gameObject.SetActive(false);
        }

        targetIndex++;

        if (targetIndex >= targets.Count)
        {
            targetIndex = 0;
        }

        target = targets[targetIndex];
        target.gameObject.SetActive(true);
    }

    private Transform GetPreviousTarget()
    {
        var previousTargetIndex = targetIndex - 1;

        if (previousTargetIndex < 0)
        {
            previousTargetIndex = targets.Count - 1;
        }

        return targets[previousTargetIndex];
    }

    private Transform GetBehindTarget()
    {
        var previousTargetIndex = targetIndex - 1;

        if (previousTargetIndex < 0)
        {
            previousTargetIndex = targets.Count - 1;
        }

        var behindTargetIndex = previousTargetIndex - 1;

        if (behindTargetIndex < 0)
        {
            behindTargetIndex = targets.Count - 1;
        }

        return targets[behindTargetIndex];
    }

    private float CheckRay(Transform sensor)
    {
        RaycastHit hit;
        if (Physics.Raycast(sensor.position, sensor.forward, out hit, rayDistance, obstacleLayer))
        {
            return hit.distance / rayDistance;
        }

        return 1.0f;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < carSensors.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(carSensors[i].position, carSensors[i].position + carSensors[i].forward * rayDistance);
        }
    }

    private void SetCarReward(float reward)
    {
        SetReward(reward);
    }
}