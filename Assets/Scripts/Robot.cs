using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Geometry;
// using System.Linq;

public class Robot : MonoBehaviour
{
    public struct Action
    {
        public List<float> voltage;
    }
    public MotionSensor baseLinkM;
    public List<MotionSensor> wheelM;
    List<MotorMoveForward> motorListMF;
    public State ROS2State;
    Vector3 targetPosition;
    public LidarSensor lidar;

    void Awake()
    {
        baseLinkM = Util.GetOrAddComponent<MotionSensor>(transform, "base_link");

        // Get data from wheels.
        wheelM = new List<MotionSensor>() {
            Util.GetOrAddComponent<MotionSensor>(transform, "left_back_forward_wheel"),
            Util.GetOrAddComponent<MotionSensor>(transform, "right_back_forward_wheel"),
        };

        // Set speed/voltage to wheels.
        motorListMF = new List<MotorMoveForward>() {
            Util.GetOrAddComponent<MotorMoveForward>(transform, "left_back_forward_wheel"),
            Util.GetOrAddComponent<MotorMoveForward>(transform, "right_back_forward_wheel"),
        };
    }

    public State GetState(Vector3 newTarget)
    {
        Vector3 carPos = baseLinkM.x;
        Vector3 carVel = baseLinkM.v;
        Vector3 carAngV = baseLinkM.AngularV;
        Quaternion carQ = baseLinkM.q;
        carQ.x = -carQ.x;
        carQ.z = -carQ.z;

        Vector3 angVLB = wheelM[0].AngularV;
        Vector3 angVRB = wheelM[1].AngularV;

        List<float> range = lidar.GetRange();

        // 將每個 lidar 座標轉換成 ros 座標
        var rangeDirection = lidar.GetRangeDirection();
        for (int i = 0; i < rangeDirection.Count; i++)
        {
            rangeDirection[i] = ToRosVec(rangeDirection[i]);
        }

        State ROS2State = new State()
        {
            ROS2TargetPosition = ToRosVec(newTarget),
            ROS2CarPosition = ToRosVec(carPos),
            ROS2CarQuaternion = ToRosQuaternion(carQ),
            ROS2WheelAngularVelocityLeftBack = ToRosVec(angVLB),
            ROS2WheelAngularVelocityRightBack = ToRosVec(angVRB),
            ROS2Range = range.ToArray(),
            ROS2RangePosition = rangeDirection.ToArray(),
        };

        return ROS2State;
    }

    public void DoAction(Action action)
    {
        motorListMF[0].SetVoltage((float)action.voltage[0]);
        motorListMF[1].SetVoltage((float)action.voltage[1]);

        // 測試用
        // motorListMF[0].SetVoltage((float)2000f);
        // motorListMF[1].SetVoltage((float)2000f);
    }

    Vector3 ToRosVec(Vector3 position)
    {
        PointMsg ROS2Position = position.To<FLU>();
        position = new Vector3((float)ROS2Position.x, (float)ROS2Position.y, (float)ROS2Position.z);

        return position;
    }

    Quaternion ToRosQuaternion(Quaternion quaternion)
    {
        QuaternionMsg ROS2Quaternion = quaternion.To<FLU>();
        quaternion = new Quaternion((float)ROS2Quaternion.x, (float)ROS2Quaternion.y, (float)ROS2Quaternion.z, (float)ROS2Quaternion.w);
        quaternion.To<FLU>();

        return quaternion;
    }
}
