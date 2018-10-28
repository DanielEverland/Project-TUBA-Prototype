﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillSetter : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private FloatVariable _value;
    [SerializeField]
    private FloatVariable _max;
    [SerializeField]
    private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private bool _inverse = false;

    private void Update()
    {
        _image.fillAmount = GetValue();
    }
    private float GetValue()
    {
        if (_value.Value == 0 || _max.Value == 0)
            return 0;

        float value = Mathf.Clamp(_value.Value / _max.Value, 0, 1);
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
}