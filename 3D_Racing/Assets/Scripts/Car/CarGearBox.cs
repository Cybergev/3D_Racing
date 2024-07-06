using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGearBox : MonoBehaviour
{
    [SerializeField] private Car car;
    private CarAsset CarAsset => car.Asset;
    private CarChassis Cassis => car.Cassis;
    private CarCassisAsset CassisAsset => CarAsset.CassisAsset;
    private CarGearBoxAsset GearBoxAsset => CarAsset.GearBoxAsset;
    private CarEngine Engine => car.Engine;
    private CarEngineAsset EngineAsset => CarAsset.EngineAsset;
    public float BaseDriveRatio => car.Asset.GearBoxAsset.Gears[0];
    public float CurrentGear => car.Asset.GearBoxAsset.Gears[selectedGearIndex];
    public int selectedGearIndex { private set; get; }
    // ASGS - Automatic Shift Gear System
    public float ASGSTargetLevel { private set; get; }
    private void FixedUpdate()
    {
        AutoShiftGear();
    }
    public void UpGear()
    {
        ShiftGear(selectedGearIndex + 1);
    }
    public void DownGear()
    {
        ShiftGear(selectedGearIndex - 1);
    }
    public void ShiftToReversGear()
    {
    }
    public void ShiftToNeutral()
    {
    }
    public void ShiftGear(int gearsIndex)
    {
        gearsIndex = Mathf.Clamp(gearsIndex, 0, GearBoxAsset.Gears.Length - 1);
        selectedGearIndex = gearsIndex;
    }
    public void AutoShiftGear()
    {
        ASGSTargetLevel = EngineAsset.EngineTorqueMax / GearBoxAsset.Gears.Length / CurrentGear;
        if (Engine.EngineTorque < ASGSTargetLevel)
            UpGear();
        if (Engine.EngineTorque - ASGSTargetLevel > ASGSTargetLevel)
            DownGear();
    }
}