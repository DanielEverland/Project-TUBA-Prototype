using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableSetter : CallbackMonobehaviour
{
    [SerializeField]
    private BaseVariable _target;
    [SerializeField]
    private BaseVariable _source;
    
    protected override void OnRaised() => _target.BaseValue = _source.BaseValue;
}