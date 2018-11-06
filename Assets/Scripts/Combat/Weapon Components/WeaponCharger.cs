using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCharger : MonoBehaviour, IAttackHandlerComponent
{
    [SerializeField]
    private FloatReference _weaponChargeTime;
    [SerializeField]
    private FloatReference _currentCharge;
    [SerializeField]
    private BoolReference _weaponCanFire;

    public bool IsFullyCharged => CurrentCharge >= ChargeTime;

    private float ChargeTime => _weaponChargeTime.Value;
    private bool CanFire => _weaponCanFire.Value;
    private float CurrentCharge { get { return _currentCharge.Value; } set { _currentCharge.Value = value; } }
    
    public void Poll(WeaponInputResponse input)
    {
        if (input.FireButtonDown && CanFire)
        {
            ChargeWeapon();
        }
        else
        {
            ResetCharge();
        }
    }
    private void ChargeWeapon()
    {
        float desiredCharge = CurrentCharge + Time.deltaTime;
        CurrentCharge = Mathf.Clamp(desiredCharge, 0, ChargeTime);
    }
    private void ResetCharge() => CurrentCharge = 0;
}