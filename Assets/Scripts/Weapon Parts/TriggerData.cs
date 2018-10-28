using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger.asset", menuName = MENU_ROOT + "Trigger", order = MENU_ORDER)]
public class TriggerData : PartBase {

    public float Cooldown { get { return _cooldown; } }
    public bool UseCharge { get { return _useCharge; } }
    public float Power { get { return _power; } }
    public int AmmoCapacity { get { return _ammoCapacity; } }
    public float ReloadTime { get { return _reloadTime; } }
    public int SeekersToFire { get { return _seekersToFire; } }
    public float ChargeTime
    {
        get
        {
            if (!UseCharge)
                return 0;

            return _chargeTime;
        }
    }

    [SerializeField]
    private bool _useCharge = false;
    [SerializeField]
    private float _chargeTime = 0;
    [SerializeField]
    private float _cooldown = 0.3f;
    [SerializeField]
    private float _power = 10;
    [SerializeField]
    private int _ammoCapacity = 10;
    [SerializeField]
    private float _reloadTime = 1;
    [SerializeField]
    private int _seekersToFire = 1;
}
