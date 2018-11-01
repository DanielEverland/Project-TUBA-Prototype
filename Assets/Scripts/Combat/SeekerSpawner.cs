using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muzzle = Weapon.Muzzle;

public class SeekerSpawner : MonoBehaviour
{
    [SerializeField]
    private ProjectileBase _prefab;
    [SerializeField]
    private WeaponVariable _selectedWeapon;
    [SerializeField]
    private Transform _weaponDirection;
    [SerializeField]
    private GameEvent _onFire;

    private static readonly Vector3 _rotationAnchor = new Vector3(0, 0, 1);

    public void SpawnSeeker(Muzzle muzzle)
    {
        ProjectileBase instance = Instantiate(_prefab);
        instance.Initialize(_selectedWeapon.Value);

        instance.transform.position = _weaponDirection.position;
        instance.transform.eulerAngles = _rotationAnchor * GetAngle(muzzle);

        _onFire.Raise();
    }
    private float GetAngle(Muzzle muzzle)
    {
        float rootAngle = GetRootAngle(muzzle);
        return ApplyInterval(muzzle, rootAngle);
    }
    private float GetRootAngle(Muzzle muzzle)
    {
        if(muzzle.Space == Space.Self)
        {
            return _weaponDirection.eulerAngles.z + muzzle.Angle;
        }
        else
        {
            return muzzle.Angle;
        }
    }
    private float ApplyInterval(Muzzle muzzle, float rawAngle)
    {
        float randomValue = Random.Range(muzzle.IntervalStart, muzzle.IntervalEnd);

        return rawAngle + randomValue;
    }
}