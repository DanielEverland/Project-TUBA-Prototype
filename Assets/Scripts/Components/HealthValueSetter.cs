using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthValueSetter : CallbackMonobehaviour {

    [SerializeField]
    private HealthVariable _target;
    [SerializeField]
    private Health _value;

    protected override void OnRaised() => _target.Value = _value;
}
