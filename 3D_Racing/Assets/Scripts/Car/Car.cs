using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CarChassis))]
public class Car : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float mass;
    [SerializeField] private CarChassis cassis;
    [SerializeField] private float motorToeque;
    [SerializeField] private float steerAngle;
    [SerializeField] private float boostModifier;
    [SerializeField] private float brakeToeque;
    private float motorForce;
    private float steerForce;
    private float boostForce;
    private float brakeForce;
    private void Start()
    {
        rigid.mass = mass;
    }
    private void FixedUpdate()
    {
        cassis?.UpdateChassisStatus(motorToeque * motorForce, steerAngle * steerForce, boostModifier * boostForce, brakeToeque * brakeForce);
    }
    public void UpdateForces(float motor, float steer, float boost, float brake)
    {
        motorForce = motor;
        steerForce = steer;
        boostForce = boost;
        brakeForce = brake;
    }
}