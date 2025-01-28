using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    public Transform target;
    public CarControl carControl;
    public Transform head;
    public Transform tail;

    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < -0.1f)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0, 0);
        }

        // Move the target to a new spot
        target.localPosition = new Vector3(Random.value * 20 - 4, 0, Random.value * 20 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target positions
        sensor.AddObservation(target.localPosition);

        // Agent head and tail positions
        var headPos = transform.parent.InverseTransformPoint(head.position);
        var tailPos = transform.parent.InverseTransformPoint(tail.position);
        sensor.AddObservation(headPos);
        sensor.AddObservation(tailPos);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        carControl.UpdateCar(controlSignal.x, controlSignal.z);

        // Distance
        var headPos = transform.parent.InverseTransformPoint(head.position);
        var tailPos = transform.parent.InverseTransformPoint(tail.position);
        float distanceToHead = Vector3.Distance(headPos, target.localPosition);
        float distanceToTail = Vector3.Distance(tailPos, target.localPosition);

        // Reached target by head
        if (distanceToHead < 2f)
        {
            SetReward(1f);
            EndEpisode();
        }
        // Reached target by tail
        else if (distanceToTail < 2f)
        {
            EndEpisode();
        }
        // Fell off platform
        else if (this.transform.localPosition.y <= -0.1f)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}