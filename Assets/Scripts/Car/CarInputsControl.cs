using UnityEngine;

public class CarInputsControl : MonoBehaviour
{
    [SerializeField] private CarInputAsset inputAsset;
    [SerializeField] private Car car;
    private void FixedUpdate()
    {
        UpdateInputs();
    }
    private void UpdateInputs()
    {
        car?.UpdateForces
        (
            (Input.GetKey(inputAsset.MotorForwardKey) ? 1 : 0) + (Input.GetKey(inputAsset.MotorBackKey) ? -1 : 0),
            (Input.GetKey(inputAsset.SteerRightKey) ? 1 : 0) + (Input.GetKey(inputAsset.SteerLeftKey) ? -1 : 0),
            (Input.GetKey(inputAsset.BoostKey) ? 1 : 0),
            (Input.GetKey(inputAsset.BrakeKey) || Mathf.Sign(car.WheelSpeed) != Mathf.Sign(car.MotorForce) && Mathf.Abs(car.WheelSpeed) > 1f ? 1 : 0)
        );
    }
}
