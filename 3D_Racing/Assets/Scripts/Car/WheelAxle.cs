using UnityEngine;
public enum WheelType
{
    Motor = 0,
    Steer = 1,
    Combined = 3
}
[System.Serializable]
public class WheelAxle
{
    [SerializeField] private WheelType type;
    [SerializeField] private WheelCollider rightWheelCollider;
    [SerializeField] private WheelCollider leftWheelCollider;
    [SerializeField] private Transform rightWheelTransfom;
    [SerializeField] private Transform leftWheelTransfom;
    private float wheelBaseWidth
    {
        get
        {
            return Vector3.Distance(rightWheelTransfom.position, leftWheelTransfom.position);
        }
    }

    public WheelType Type => type;
    public WheelCollider RightWheelCollider => rightWheelCollider;
    public WheelCollider LeftWheelCollider => leftWheelCollider;
    public Transform RightWheelTransfom => rightWheelTransfom;
    public Transform LeftWheelTransfom => leftWheelTransfom;

    public void UpdateWheelStatus(float motor, float steer, float boost, float brake, float wheelBaseLenght)
    {
        ApplyAntiRole();
        ApplyDownForce();
        CorrectStiffness();
        ApplyMotor(motor, boost);
        ApplySteer(steer, wheelBaseLenght);
        ApplyBrake(brake);
        SyncMechTransform();
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

    private void ApplyMotor(float motor, float boost)
    {
        if (type != WheelType.Combined && type != WheelType.Motor)
            return;
        rightWheelCollider.motorTorque = motor * (boost >= 1 ? boost : 1);
        leftWheelCollider.motorTorque = motor * (boost >= 1 ? boost : 1);
    }

    private void ApplySteer(float steer, float wheelBaseLenght)
    {
        if (type != WheelType.Combined && type != WheelType.Steer)
            return;
        float radius = Mathf.Abs(wheelBaseLenght * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steer))));
        float angleSing = Mathf.Sign(steer);
        if (angleSing > 0)
        {
            rightWheelCollider.steerAngle = Mathf.Deg2Rad * Mathf.Atan(wheelBaseLenght / (radius - (wheelBaseWidth * 0.5f))) * angleSing;
            leftWheelCollider.steerAngle = Mathf.Deg2Rad * Mathf.Atan(wheelBaseLenght / (radius + (wheelBaseWidth * 0.5f))) * angleSing;
        }
        else if (angleSing < 0)
        {
            rightWheelCollider.steerAngle = Mathf.Deg2Rad * Mathf.Atan(wheelBaseLenght / (radius + (wheelBaseWidth * 0.5f))) * angleSing;
            leftWheelCollider.steerAngle = Mathf.Deg2Rad * Mathf.Atan(wheelBaseLenght / (radius - (wheelBaseWidth * 0.5f))) * angleSing;
        }
        else
        {
            rightWheelCollider.steerAngle = 0;
            leftWheelCollider.steerAngle = 0;
        }
        Debug.Log(nameof(wheelBaseWidth) + $" {wheelBaseWidth}");
        Debug.Log(nameof(wheelBaseLenght) + $" {wheelBaseLenght}");
        Debug.Log(nameof(radius) + $" {radius}");
        Debug.Log(nameof(angleSing) + $" {angleSing}");
    }
    private void ApplyBrake(float brake)
    {
        rightWheelCollider.brakeTorque = brake;
        leftWheelCollider.brakeTorque = brake;
    }
    private void CorrectStiffness()
    {
    }
    private void ApplyDownForce()
    {
    }
    private void ApplyAntiRole()
    {
    }
}
