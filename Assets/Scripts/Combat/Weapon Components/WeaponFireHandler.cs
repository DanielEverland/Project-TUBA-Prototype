using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Muzzle = Weapon.Muzzle;

using static UnityEngine.Mathf;

public class WeaponFireHandler : MonoBehaviour
{
    [SerializeField]
    private IntReference _currentAmmoCount;
    [SerializeField]
    private IntReference _maxAmmoCount;
    [SerializeField]
    private FloatReference _lastFireTime;
    [SerializeField]
    private SeekerSpawner _seekerSpawner;
    [SerializeField]
    private WeaponVariable _selectedWeapon;

    public float WeaponLastFire { get { return _lastFireTime.Value; } set { _lastFireTime.Value = value; } }
    public int CurrentAmmo
    {
        get
        {
            return _selectedWeapon.Value.CurrentAmmo;
        }
        set
        {
            _selectedWeapon.Value.CurrentAmmo = value;
            _currentAmmoCount.Value = _selectedWeapon.Value.CurrentAmmo;
        }
    }

    private IEnumerable<Muzzle> Muzzles => _selectedWeapon.Value.TriggerData.Muzzles;
    private int SeekersToFire => _selectedWeapon.Value.TriggerData.SeekersToFire;

    public void Fire()
    {
        WeaponLastFire = Time.time;

        CurrentAmmo = Clamp(CurrentAmmo - 1, 0, _maxAmmoCount.Value);

        if(_selectedWeapon.Value.TriggerData.RandomMuzzle)
        {
            FireRandom();
        }
        else
        {
            FireAll();
        }
    }
    private void FireRandom()
    {
        List<Muzzle> allMuzzles = new List<Muzzle>(Muzzles);

        for (int i = 0; i < SeekersToFire; i++)
        {
            if (allMuzzles.Count == 0)
                break;

            Muzzle selected = allMuzzles.Random();
            allMuzzles.Remove(selected);

            _seekerSpawner.SpawnSeeker(selected);
        }
        
    }
    private void FireAll()
    {
        foreach (Muzzle muzzle in _selectedWeapon.Value.TriggerData.Muzzles)
        {
            _seekerSpawner.SpawnSeeker(muzzle);
        }
    }
}