using UnityEngine;

public class CarInputsControl : MonoBehaviour
{
    [SerializeField] private InputAsset inputAsset;
    [SerializeField] private Car car;
    private void FixedUpdate()
    {
        UpdateInputs();
    }
    private void UpdateInputs()
    {
        float motor = 0;
        float steer = 0;
        float boost = 0;
        float breake = 0;
        motor += (Input.GetKey(inputAsset.MotorForwardKey) ? 1 : 0) + (Input.GetKey(inputAsset.MotorBackKey) ? -1 : 0);
        steer += (Input.GetKey(inputAsset.SteerRightKey) ? 1 : 0) + (Input.GetKey(inputAsset.SteerLeftKey) ? -1 : 0);
        boost += (Input.GetKey(inputAsset.BoostKey) ? 1 : 0);
        breake += (Input.GetKey(inputAsset.BrakeKey) ? 1 : 0);
        car?.UpdateForces(motor, steer, boost, breake);
    }
}
