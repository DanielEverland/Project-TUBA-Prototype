using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger.asset", menuName = MENU_ROOT + "Trigger", order = MENU_ORDER)]
public class TriggerData : PartBase {

    public float Cooldown { get { return _cooldown; } }

    [SerializeField, Range(0, 10)]
    private float _cooldown = 0.3f;
}
