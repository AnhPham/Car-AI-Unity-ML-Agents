# AI Test 1 #

### Mac OS ###

#### Activate Virtual Environment ####

```console
cd /Users/anhpham/Works/Python
source mlagents-env/bin/activate
```

#### Deactivate Virtual Environment ####

```console
deactivate
```

#### Resume training ####

```console
cd /Users/anhpham/Works/Projects/test-ai1/AITest1
mlagents-learn config/car_config.yaml --run-id=Car --resume
```

#### Force training from beginning ####

```console
cd /Users/anhpham/Works/Projects/test-ai1/AITest1
mlagents-learn config/car_config.yaml --run-id=Car --force
```

### Windows ###

#### Activate Virtual Environment ####

```console
cd C:\Users\anhpt\Works\Python
.venv\Scripts\activate
```

#### Deactivate Virtual Environment ####

```console
deactivate
```

#### Resume training ####

```console
cd C:\Users\anhpt\Works\Projects\test-ai1\AITest1
mlagents-learn config/car_config.yaml --run-id=Car --torch-device=cuda --resume
```

#### Force training from beginning ####

```console
cd C:\Users\anhpt\Works\Projects\test-ai1\AITest1
mlagents-learn config/car_config.yaml --run-id=Car --torch-device=cuda --force
```