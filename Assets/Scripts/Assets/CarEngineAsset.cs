using UnityEngine;
[CreateAssetMenu]
public class CarEngineAsset : ScriptableObject
{
    [SerializeField] private AnimationCurve engineTorqueCurve;
    [SerializeField] private float engineTorqueMax;
    [SerializeField] private float engineRpmMax;
    [SerializeField] private float engineRpmMin;
    [SerializeField] private AnimationCurve boostCurve;
    [SerializeField] private float boostModifierMax;
    public AnimationCurve EngineTorqueCurve => engineTorqueCurve;
    public float EngineTorqueMax => engineTorqueMax;
    public float EngineRpmMax => engineRpmMax;
    public float EngineRpmMin => engineRpmMin;
    public AnimationCurve BoostCurve => boostCurve;
    public float BoostModifierMax => boostModifierMax;
}
