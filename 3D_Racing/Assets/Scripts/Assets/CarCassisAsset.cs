using UnityEngine;
[CreateAssetMenu]
public class CarCassisAsset : ScriptableObject
{
    [SerializeField] private AnimationCurve steerAngleCurve;
    [SerializeField] private float steerAngleMax;
    [SerializeField] private AnimationCurve brakeTorqueCurve;
    [SerializeField] private float brakeTorqueMax;
    [SerializeField] private AnimationCurve linearDragCurve;
    [SerializeField] private float linearDragMax;
    [SerializeField] private AnimationCurve angularDragCurve;
    [SerializeField] private float angularDragMax;
    [SerializeField] private AnimationCurve downForceCurve;
    [SerializeField] private float downForceMax;

    public AnimationCurve SteerAngleCurve => steerAngleCurve;
    public float SteerAngleMax => steerAngleMax;
    public AnimationCurve BrakeTorqueCurve => brakeTorqueCurve;
    public float BrakeTorqueMax => brakeTorqueMax;
    public AnimationCurve LinearDragCurve => linearDragCurve;
    public float LinearDragMax => linearDragMax;
    public AnimationCurve AngularDragCurve => angularDragCurve;
    public float AngularDragMax => angularDragMax;
    public AnimationCurve DownForceCurve => downForceCurve;
    public float DownForceMax => downForceMax;
}
