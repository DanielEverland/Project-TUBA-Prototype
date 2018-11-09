using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2DSetter : CallbackMonobehaviour {

    [SerializeField]
    private CharacterController2DVariable _target;
    [SerializeField]
    private CharacterController2D _value;

    protected override void OnRaised() => _target.Value = _value;
}
