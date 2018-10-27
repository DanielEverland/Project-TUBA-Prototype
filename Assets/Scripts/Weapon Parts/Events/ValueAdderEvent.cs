using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValueAdderEvent", menuName = MENU_ROOT + "Events/Adder", order = MENU_ORDER)]
public class ValueAdderEvent : EventData
{
    [SerializeField]
    private FloatReference _target;
    [SerializeField]
    private FloatReference _amount;

    public override void OnEventRaised()
    {
        _target.Value += _amount.Value;
    }
}