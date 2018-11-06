using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muzzle = Weapon.Muzzle;

[CreateAssetMenu(fileName = "Trigger.asset", menuName = MENU_ROOT + "Trigger", order = MENU_ORDER)]
public class TriggerData : PartBase {

    public IEnumerable<Muzzle> Muzzles => _muzzles;
    public bool RandomMuzzle => _randomMuzzle;
    public float Cooldown => _cooldown;
    public bool UseCharge => _useCharge;
    public float Power => _power;
    public int AmmoCapacity => _ammoCapacity;
    public float ReloadTime => _reloadTime;
    public int SeekersToFire => _seekersToFire;
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
    private List<Muzzle> _muzzles;
    [SerializeField]
    private bool _randomMuzzle = false;
    [SerializeField]
    private int _seekersToFire = 1;

    private void Reset()
    {
        if(_muzzles == null)
            _muzzles = new List<Muzzle>();
        
        if(_muzzles.Count == 0)
        {
            _muzzles.Add(new Muzzle());
        }
    }
}
