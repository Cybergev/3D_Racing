using UnityEngine;
[CreateAssetMenu]
public class CarAsset : ScriptableObject
{
    [SerializeField] private float mass;
    [SerializeField] private float speedMax;
    [SerializeField] private CarCassisAsset cassisAsset;
    [SerializeField] private CarGearBoxAsset gearBoxAsset;
    [SerializeField] private CarEngineAsset engineAsset;
    public float Mass => mass;
    public float SpeedMax => speedMax;
    public CarCassisAsset CassisAsset => cassisAsset;
    public CarGearBoxAsset GearBoxAsset => gearBoxAsset;
    public CarEngineAsset EngineAsset => engineAsset;
}
