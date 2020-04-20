# Setup
## ROS (Ubuntu)
Start rosbridge websocket werver and note the IP address of this machine.
```
roslaunch rosbridge_server rosbridge_websocket
```
Start a `geometry_msgs/PoseStamped` publisher. Sample command line command:
```
rostopic pub /pose geometry_msgs/PoseStamped "header:
  seq: 0
  stamp:
    secs: 0
    nsecs: 0
  frame_id: 'cube'
pose:
  position:
    x: 0.0
    y: 5.0
    z: 20.0
  orientation:
    x: 0.0
    y: 0.0
    z: 0.0
    w: 1.0"
```
## Unity
In the `SampleScene` select `PoseSubscriber` object.
1. In the Inspector for the `RosConnector` script enter Ros Bridge Server Url (with the IP of the machine above), default port is 9090
2. For the `PoseSubscriber` script enter the name of the topic. It's `pose` in case the command line example from above is used.
3. GameObjects to track are automatically detected and updated based on the `frame_id` field in the header of every ROS message. Therefore, make sure the name of the tracked GameObject and the `frame_id` in the ROS message are identical.
Required transform between Unity and ROS coordinate systems is already done in the `PoseSubcriber` script.
