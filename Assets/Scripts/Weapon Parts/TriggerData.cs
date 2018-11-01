using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muzzle = Weapon.Muzzle;

[CreateAssetMenu(fileName = "Trigger.asset", menuName = MENU_ROOT + "Trigger", order = MENU_ORDER)]
public class TriggerData : PartBase {

    public IEnumerable<Muzzle> Muzzles { get { return _muzzles; } }
    public bool RandomMuzzle { get { return _randomMuzzle; } }
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
