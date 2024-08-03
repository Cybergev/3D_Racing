using System;
using UnityEngine;
public enum AxleType
{
    Simple = 0,
    Motor = 1,
    Steer = 2,
    Combined = 3
}
[Serializable]
public class WheelAxle
{
    [SerializeField] private AxleType axleType;
    [SerializeField] private CarChassis chassis;
    [SerializeField] private WheelCollider rightWheelCollider;
    [SerializeField] private WheelCollider leftWheelCollider;
    [SerializeField] private Transform rightWheelTransfom;
    [SerializeField] private Transform leftWheelTransfom;
    [SerializeField] private float additionalWheelDownForce;
    [SerializeField] private float antiRoleForce;
    [SerializeField] private float baseForwardStiffnes = 1.5f;
    [SerializeField] private float stabilityForwardFactor = 1.0f;
    [SerializeField] private float stabilitySidewaysFactor = 1.0f;
    [SerializeField] private float baseSidewaysStiffnes = 2.0f;
    #region PublicFields
    public AxleType AxleType => axleType;
    public WheelCollider RightWheelCollider => rightWheelCollider;
    public WheelCollider LeftWheelCollider => leftWheelCollider;
    public Transform RightWheelTransfom => rightWheelTransfom;
    public Transform LeftWheelTransfom => leftWheelTransfom;
    public WheelHit RighWheeltHit
    {
        get
        {
            WheelHit hit;
            rightWheelCollider.GetGroundHit(out hit);
            return hit;
        }
    }
    public WheelHit LeftWheelHit
    {
        get
        {
            WheelHit hit;
            leftWheelCollider.GetGroundHit(out hit);
            return hit;
        }
    }
    public float WheelBaseWidth
    {
        get
        {
            return Vector3.Distance(rightWheelTransfom.position, leftWheelTransfom.position);
        }
    }
    public float WheelRadius
    {
        get
        {
            return (rightWheelCollider.radius + leftWheelCollider.radius) / 2;
        }
    }
    public float WheelRpm
    {
        get
        {
            return (rightWheelCollider.rpm + leftWheelCollider.rpm) / 2;
        }
    }
    public float WheelSpeed
    {
        get
        {
            return WheelRpm * WheelRadius * 2 * 0.1885f;
        }
    }
    #endregion
    public void ConfigurateVehicleSubsteps(float speedThreshold, int speedBelowThreshold, int stepsAboveThreshold)
    {
        rightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
        leftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
    }
    public void UpdateWheelStatus(float motor, float steer, float boost, float brake, float wheelBaseLenght)
    {
        ApplyMotor(motor, boost);
        ApplySteer(steer, wheelBaseLenght);
        ApplyBrake(brake);
        ApplyAntiRole();
        ApplyDownForce();
        CorrectStiffness();
        SyncMechTransform();
    }

    private void ApplyMotor(float motor, float boost)
    {
        if (AxleType != AxleType.Combined && AxleType != AxleType.Motor)
            return;
        rightWheelCollider.motorTorque = motor * (boost >= 1 ? boost : 1);
        leftWheelCollider.motorTorque = motor * (boost >= 1 ? boost : 1);
    }

    private void ApplySteer(float steer, float wheelBaseLenght)
    {
        if (AxleType != AxleType.Combined && AxleType != AxleType.Steer)
            return;
        float radius = Mathf.Abs(wheelBaseLenght * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steer))));
        float angleSing = Mathf.Sign(steer);
        if (angleSing > 0)
        {
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (WheelBaseWidth * 0.5f))) * angleSing;
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (WheelBaseWidth * 0.5f))) * angleSing;
        }
        else if (angleSing < 0)
        {
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (WheelBaseWidth * 0.5f))) * angleSing;
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (WheelBaseWidth * 0.5f))) * angleSing;
        }
        else
        {
            rightWheelCollider.steerAngle = 0;
            leftWheelCollider.steerAngle = 0;
        }
    }
    private void ApplyBrake(float brake)
    {
        rightWheelCollider.brakeTorque = brake;
        leftWheelCollider.brakeTorque = brake;
    }
    private void ApplyAntiRole()
    {
        float travelR = 1.0f;
        float travelL = 1.0f;

        if (rightWheelCollider.isGrounded)
            travelR = (-rightWheelCollider.transform.InverseTransformPoint(RighWheeltHit.point).y - rightWheelCollider.radius) / rightWheelCollider.suspensionDistance;
        if (leftWheelCollider.isGrounded)
            travelL = (-leftWheelCollider.transform.InverseTransformPoint(LeftWheelHit.point).y - leftWheelCollider.radius) / leftWheelCollider.suspensionDistance;

        float forceDir = (travelR - travelL);
        if (rightWheelCollider.isGrounded)
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelCollider.transform.up * -forceDir * antiRoleForce, rightWheelCollider.transform.position);
        if (leftWheelCollider.isGrounded)
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelCollider.transform.up * forceDir * antiRoleForce, leftWheelCollider.transform.position);
    }
    private void ApplyDownForce()
    {
        if (rightWheelCollider.isGrounded)
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(RighWheeltHit.normal * -additionalWheelDownForce * rightWheelCollider.attachedRigidbody.velocity.magnitude, rightWheelCollider.transform.position);
        if (leftWheelCollider.isGrounded)
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(LeftWheelHit.normal * -additionalWheelDownForce * leftWheelCollider.attachedRigidbody.velocity.magnitude, leftWheelCollider.transform.position);
    }
    private void CorrectStiffness()
    {
        WheelFrictionCurve rightForward = rightWheelCollider.forwardFriction;
        WheelFrictionCurve lefttForward = leftWheelCollider.forwardFriction;

        WheelFrictionCurve rightSideways = rightWheelCollider.sidewaysFriction;
        WheelFrictionCurve lefttSideways = leftWheelCollider.sidewaysFriction;

        rightForward.stiffness = baseForwardStiffnes + Mathf.Abs(RighWheeltHit.forwardSlip) * stabilityForwardFactor;
        lefttForward.stiffness = baseForwardStiffnes + Mathf.Abs(LeftWheelHit.forwardSlip) * stabilityForwardFactor;

        rightSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(RighWheeltHit.sidewaysSlip) * stabilitySidewaysFactor;
        lefttSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(LeftWheelHit.sidewaysSlip) * stabilitySidewaysFactor;

        rightWheelCollider.forwardFriction = rightForward;
        leftWheelCollider.forwardFriction = lefttForward;

        rightWheelCollider.sidewaysFriction = rightSideways;
        leftWheelCollider.sidewaysFriction = lefttSideways;
    }
    private void SyncMechTransform()
    {
        Vector3 rightPosition;
        Vector3 leftPosition;
        Quaternion rightRotation;
        Quaternion leftRotation;
        rightWheelCollider.GetWorldPose(out rightPosition, out rightRotation);
        leftWheelCollider.GetWorldPose(out leftPosition, out leftRotation);
        rightWheelTransfom.transform.position = rightPosition;
        leftWheelTransfom.transform.position = leftPosition;
        rightWheelTransfom.transform.rotation = rightRotation;
        leftWheelTransfom.transform.rotation = leftRotation;
    }

}
