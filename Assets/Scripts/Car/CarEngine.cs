using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [SerializeField] private Car car;
    private CarAsset CarAsset => car.Asset;
    private CarChassis Cassis => car.Cassis;
    private CarCassisAsset CassisAsset => CarAsset.CassisAsset;
    private CarGearBox GearBox => car.GearBox;
    private CarGearBoxAsset GearBoxAsset => CarAsset.GearBoxAsset;
    private CarEngineAsset EngineAsset => CarAsset.EngineAsset;
    private AnimationCurve engineTorqueCurve => car.Asset.EngineAsset.EngineTorqueCurve;
    private float engineTorqueMax => car.Asset.EngineAsset.EngineTorqueMax;
    private float engineRpmMax => car.Asset.EngineAsset.EngineRpmMax;
    private float engineRpmMin => car.Asset.EngineAsset.EngineRpmMin;
    #region PublicFields
    public float EngineTorque { private set; get; }
    public float EngineRpm { private set; get; }
    #endregion
    private void FixedUpdate()
    {
        UpdateEngineTorque();
    }
    private void UpdateEngineTorque()
    {
        EngineRpm = engineRpmMin + Mathf.Abs(Cassis.CurrentWheelRpm * GearBox.CurrentGear * GearBox.BaseDriveRatio);
        EngineRpm = Mathf.Clamp(EngineRpm, engineRpmMin, engineRpmMax);
        EngineTorque = engineTorqueCurve.Evaluate(EngineRpm / engineRpmMax) * engineTorqueMax;
    }
}
