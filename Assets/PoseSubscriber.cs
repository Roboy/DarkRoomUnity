/*
 * adapted from
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class PoseSubscriber : UnitySubscriber<MessageTypes.Geometry.PoseStamped>
    {
        private Transform publishedTransform;
        private Vector3 position;
        private Quaternion rotation;
        private string frame_id;
        private bool isMessageReceived;

        private System.Collections.Generic.Dictionary<string, Transform> objectsMap;

        protected override void Start()
        {
            objectsMap = new System.Collections.Generic.Dictionary<string, Transform>();
            base.Start();
		}
		
        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.PoseStamped message)
        {
            position = GetPosition(message).Ros2Unity();
            rotation = GetRotation(message).Ros2Unity();
            frame_id = GetFrameId(message);
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            if (objectsMap.ContainsKey(frame_id))
            {
                objectsMap.TryGetValue(frame_id, out publishedTransform);
            }
            else
            {
                var obj = GameObject.Find(frame_id);
                if ( obj != null)
                {
                    publishedTransform = obj.transform;
                    objectsMap.Add(frame_id, publishedTransform);
                } else
                {
                    Debug.LogWarning("Unable to find a game object corresponding to the frame_id: \"" + frame_id + "\"");
                    return;
                }
                
            }

            publishedTransform.position = position;
            publishedTransform.rotation = rotation;
        }

        private Vector3 GetPosition(MessageTypes.Geometry.PoseStamped message)
        {
            return new Vector3(
                (float)message.pose.position.x,
                (float)message.pose.position.y,
                (float)message.pose.position.z);
        }

        private Quaternion GetRotation(MessageTypes.Geometry.PoseStamped message)
        {
            return new Quaternion(
                (float)message.pose.orientation.x,
                (float)message.pose.orientation.y,
                (float)message.pose.orientation.z,
                (float)message.pose.orientation.w);
        }

        private string GetFrameId(MessageTypes.Geometry.PoseStamped message)
        {
            return message.header.frame_id;
        }
    }
}