using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger.asset", menuName = MENU_ROOT + "Trigger", order = MENU_ORDER)]
public class TriggerData : PartBase {

    public float Cooldown { get { return _cooldown; } }
    public bool UseCharge { get { return _useCharge; } }
    public float Power { get { return _power; } }
    public float Capacity { get { return _capacity; } }
    public float Quantity { get { return _quantity; } }
    public float Charge
    {
        get
        {
            if (!UseCharge)
                return 0;

            return _charge;
        }
    }

    [SerializeField]
    private bool _useCharge = false;
    [SerializeField]
    private float _charge = 0;
    [SerializeField]
    private float _cooldown = 0.3f;
    [SerializeField]
    private float _power = 10;
    [SerializeField]
    private int _capacity = 10;
    [SerializeField]
    private int _quantity = 1;
}
