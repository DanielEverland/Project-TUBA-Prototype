using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotElement : MonoBehaviour {

    [SerializeField]
    private TriggerSelector _triggerSelector;

    private void Awake()
    {
        _triggerSelector.Initialize(this);
    }
    public void ChangeTrigger(TriggerData trigger)
    {
        Debug.Log(trigger);
    }
}
