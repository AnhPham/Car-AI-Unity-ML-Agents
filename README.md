# AI Test 1 #

### Mac OS ###

* Activate Virtual Environment

cd /Users/anhpham/Works/Python
source mlagents-env/bin/activate

* Deactivate Virtual Environment

deactivate

* Resume training

cd /Users/anhpham/Works/Projects/test-ai1/AITest1
mlagents-learn config/car_config.yaml --run-id=Car --resume

* Force training from beginning

cd /Users/anhpham/Works/Projects/test-ai1/AITest1
mlagents-learn config/car_config.yaml --run-id=Car --force