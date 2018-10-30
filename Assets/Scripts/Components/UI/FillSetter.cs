using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillSetter : BaseFillSetter<BaseVariable>
{
}
public abstract class BaseFillSetter<T> : MonoBehaviour where T : BaseVariable
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private T _value;
    [SerializeField]
    private T _max;
    [SerializeField]
    private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private bool _inverse = false;

    protected T Value { get { return _value; } }
    protected T Max { get { return _max; } }

    private void Update()
    {
        _image.fillAmount = GetFillValue();
    }
    protected virtual float GetFillValue()
    {
        if (GetValue() == 0 || GetMaxValue() == 0)
            return 0;
        
        float value = Mathf.Clamp(GetValue() / GetMaxValue(), 0f, 1f);
        float curveValue = _curve.Evaluate(value);

        if (_inverse)
        {
            return 1 - curveValue;
        }
        else
        {
            return curveValue;
        }
    }
    protected virtual float GetValue()
    {
        return System.Convert.ToSingle(_value.BaseValue);
    }
    protected virtual float GetMaxValue()
    {
        return System.Convert.ToSingle(_max.BaseValue);
    }
}