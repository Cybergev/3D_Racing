using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class CarChassis : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private List<WheelAxle> wheels = new List<WheelAxle>();
    #region PublicFields
    private CarAsset CarAsset => car.Asset;
    private CarCassisAsset CassisAsset => CarAsset.CassisAsset;
    private CarEngine Engine => car.Engine;
    private CarEngineAsset EngineAsset => CarAsset.EngineAsset;
    private CarGearBox GearBox => car.GearBox;
    private CarGearBoxAsset GearBoxAsset => CarAsset.GearBoxAsset;
    private AnimationCurve SteerAngleCurve => CassisAsset.SteerAngleCurve;
    private float SteerAngleMax => CassisAsset.SteerAngleMax;
    private AnimationCurve BrakeToequeCurve => CassisAsset.BrakeTorqueCurve;
    private float BrakeToequeMax => CassisAsset.BrakeTorqueMax;
    private AnimationCurve LinearDragCurve => CassisAsset.LinearDragCurve;
    private float LinearDragMax => CassisAsset.LinearDragMax;
    private AnimationCurve AngularDragCurve => CassisAsset.AngularDragCurve;
    private float AngularDragMax => CassisAsset.AngularDragMax;
    private AnimationCurve DownForceCurve => CassisAsset.DownForceCurve;
    private float DownForceMax => CassisAsset.DownForceMax;
    public float CurrentWheelRpm
    {
        get
        {
            float result = 0;
            foreach (var wheel in wheels)
                result += wheel.WheelRpm;
            return result / wheels.Count;
        }
    }
    public float CurrentWheelSpeed
    {
        get
        {
            float result = 0;
            foreach (var wheel in wheels)
                result += wheel.WheelSpeed;
            return result / wheels.Count;
        }
    }
    public float wheelBaseLenght
    {
        get
        {
            return Vector3.Distance(wheels[0].RightWheelTransfom.position, wheels[wheels.Count - 1].RightWheelTransfom.position);
        }
    }
    #endregion

    private void Start()
    {
        car.Rigid.centerOfMass = car.CentrOfMass ? car.CentrOfMass.localPosition : car.Rigid.centerOfMass;
        wheels.ForEach((wheel) => wheel.ConfigurateVehicleSubsteps(50, 50, 50));
    }
    private void FixedUpdate()
    {
        UpdateChassisStatus
        (
            car.Engine.EngineTorque * car.MotorForce,
            (SteerAngleCurve.Evaluate(car.Speed / CarAsset.SpeedMax) * SteerAngleMax) * car.SteerForce,
            (CarAsset.EngineAsset.BoostCurve.Evaluate(car.Speed / CarAsset.SpeedMax) * EngineAsset.BoostModifierMax) * car.BoostForce,
            (BrakeToequeCurve.Evaluate(car.Speed / CarAsset.SpeedMax) * BrakeToequeMax) * car.BrakeForce
        );
        UpdatelinearDrag();
        UpdateAngularDrag();
        UpdateDownForce();
    }

    private void UpdatelinearDrag()
    {
        car.Rigid.drag = LinearDragCurve.Evaluate(car.Speed / CarAsset.SpeedMax) * LinearDragMax;
    }

    private void UpdateAngularDrag()
    {
        car.Rigid.angularDrag = AngularDragCurve.Evaluate(car.Speed / CarAsset.SpeedMax) * AngularDragMax;
    }

    private void UpdateDownForce()
    {
        car.Rigid.AddForce(-transform.up * DownForceCurve.Evaluate(car.Speed / CarAsset.SpeedMax) * DownForceMax);
    }

    public void UpdateChassisStatus(float motor, float steer, float boost, float brake)
    {
        float motorWheels = 0;
        wheels.ForEach((wheel) => motorWheels += wheel.AxleType == AxleType.Combined || wheel.AxleType == AxleType.Motor ? 2 : 0);
        wheels.ForEach((wheel) => wheel.UpdateWheelStatus(motor / motorWheels, steer, boost, brake, wheelBaseLenght));
    }
}
