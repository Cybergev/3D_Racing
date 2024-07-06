using UnityEngine;
using UnityEngine.UI;

public class UiFillAmountController : MonoBehaviour
{
    [SerializeField] protected Image[] targetExitFill;
    protected float FillAmount
    {
        set
        {
            foreach (var image in targetExitFill)
                image.fillAmount = value;
        }
    }
    public virtual void OnChangeTargetValueAmount(float value)
    {
        FillAmount = value;
    }
    public virtual void OnChangeTargetValueAmount(int value)
    {
        OnChangeTargetValueAmount(value);
    }
    public virtual void OnChangeTargetValueAmount(string value)
    {
        OnChangeTargetValueAmount(float.Parse(value));
    }
}
