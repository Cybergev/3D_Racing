using UnityEngine;
[CreateAssetMenu]
public class CarGearBoxAsset : ScriptableObject
{
    [SerializeField] private float[] gears;
    [SerializeField] private float rearGear;

    public float[] Gears => gears;
    public float RearGear => rearGear;
}
