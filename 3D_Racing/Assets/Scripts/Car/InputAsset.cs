using UnityEngine;
[CreateAssetMenu]
public class InputAsset : ScriptableObject
{
    [SerializeField] private KeyCode motorForwardKey;
    [SerializeField] private KeyCode motorBackKey;
    [SerializeField] private KeyCode steerRightKey;
    [SerializeField] private KeyCode steerLeftKey;
    [SerializeField] private KeyCode boostKey;
    [SerializeField] private KeyCode brakeKey;

    public KeyCode MotorForwardKey => motorForwardKey;
    public KeyCode MotorBackKey => motorBackKey;
    public KeyCode SteerRightKey => steerRightKey;
    public KeyCode SteerLeftKey => steerLeftKey;
    public KeyCode BoostKey => boostKey;
    public KeyCode BrakeKey => brakeKey;
}
