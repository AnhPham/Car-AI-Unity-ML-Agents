# Car AI - Unity ML #

### Demo ###

<p align="center">
  <img width="500px" src="/learn/unity/ai/car-ai-demo.gif?raw=true" alt="Demo">
</p>

### Training ###

<p align="center">
  <img width="500px" src="/learn/unity/ai/car-ai-training.gif?raw=true" alt="Demo">
</p>

### General ###

#### Install Unity 2023.2 or higher & Python 3.10.12 ####

https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md

#### Create Virtual Environment ####

https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Using-Virtual-Environment.md

### Training AI on Mac OS ###

#### Activate Virtual Environment ####

```console
source [PATH TO VIRTUAL ENV]/bin/activate
```

#### Resume training ####

```console
cd [PATH TO UNITY PROJECT]
mlagents-learn config/car_config.yaml --run-id=Car --resume
```

#### Force training from beginning ####

```console
cd [PATH TO UNITY PROJECT]
mlagents-learn config/car_config.yaml --run-id=Car --force
```

### Training AI on Windows ###

#### Activate Virtual Environment ####

```console
[PATH TO VIRTUAL ENV]\Scripts\activate
```

#### Resume training ####

```console
cd [PATH TO UNITY PROJECT]
mlagents-learn config/car_config.yaml --run-id=Car --resume --torch-device=cuda 
```

#### Force training from beginning ####

```console
cd [PATH TO UNITY PROJECT]
mlagents-learn config/car_config.yaml --run-id=Car --force --torch-device=cuda
```

### Using new AI Training Model ###

Copy file from [PATH TO UNITY PROJECT]/results/Car/Car.onnx to [PATH TO UNITY PROJECT]/Assets/Models/Car.onnx