using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChassis : MonoBehaviour
{
    [SerializeField] private List<WheelAxle> wheels = new List<WheelAxle>();
    private float wheelBaseLenght
    {
        get
        {
            return Vector3.Distance(wheels[0].RightWheelTransfom.position, wheels[wheels.Count - 1].RightWheelTransfom.position);
        }
    }
    public void UpdateChassisStatus(float motor, float steer, float boost, float brake)
    {
        foreach (var wheel in wheels)
            wheel.UpdateWheelStatus(motor, steer, boost, brake, wheelBaseLenght);
    }
}
