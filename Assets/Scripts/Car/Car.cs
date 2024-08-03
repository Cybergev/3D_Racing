using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [SerializeField] private CarAsset asset;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform centrOfMass;
    [SerializeField] private CarChassis cassis;
    [SerializeField] private CarGearBox gearBox;
    [SerializeField] private CarEngine engine;
    [SerializeField] private UnityEvent<int> speedUpdateEvent;
    [SerializeField] private UnityEvent<int> egineTorqueUpdateEvent;
    [SerializeField] private UnityEvent<int> engineRmpUpdateEvent;
    [SerializeField] private UnityEvent<int> ASGSTargetUpdateEvent;
    [SerializeField] private UnityEvent<int> gearIndexUpdateEvent;
    #region PublicFields
    public CarAsset Asset => asset;
    public Rigidbody Rigid => rigid;
    public Transform CentrOfMass => centrOfMass;
    public CarChassis Cassis => cassis;
    public CarGearBox GearBox => gearBox;
    public CarEngine Engine => engine;
    public float Speed => rigid.velocity.magnitude * 3.6f;
    public float WheelSpeed => cassis.CurrentWheelSpeed;
    public float MotorForce { private set; get; }
    public float SteerForce { private set; get; }
    public float BoostForce { private set; get; }
    public float BrakeForce { private set; get; }
    #endregion
    private void Start()
    {
        rigid.mass = asset.Mass;
    }
    private void FixedUpdate()
    {
        speedUpdateEvent.Invoke((int)Speed);
        egineTorqueUpdateEvent.Invoke((int)engine.EngineTorque);
        engineRmpUpdateEvent.Invoke((int)(engine.EngineRpm / asset.EngineAsset.EngineTorqueMax));
        ASGSTargetUpdateEvent.Invoke((int)gearBox.ASGSTargetLevel);
        gearIndexUpdateEvent.Invoke(gearBox.selectedGearIndex);
    }
    public void UpdateForces(float motor, float steer, float boost, float brake)
    {
        MotorForce = motor;
        SteerForce = steer;
        BoostForce = boost;
        BrakeForce = brake;
    }
}