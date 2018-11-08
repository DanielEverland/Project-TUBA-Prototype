using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectValueSetter : CallbackMonobehaviour {

    [SerializeField]
    private GameObjectVariable _target;
    [SerializeField]
    private GameObject _value;

    protected override void OnRaised() => _target.Value = _value;
}
