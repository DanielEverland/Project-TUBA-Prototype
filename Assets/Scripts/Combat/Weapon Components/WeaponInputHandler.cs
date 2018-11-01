using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInputHandler : MonoBehaviour
{
    [SerializeField]
    private IntReference _currentAmmo;
    [SerializeField]
    private FloatReference _cooldownTimeProperty;
    [SerializeField]
    private FloatReference _currentCooldown;
    [SerializeField]
    private BoolReference _weaponCanFire;
    [SerializeField]
    private Vector3Reference _direction;

    private float CurrentCooldown { get { return _currentCooldown.Value; } }
    private float CooldownTime { get { return _cooldownTimeProperty.Value; } }
    private int CurrentAmmo { get { return _currentAmmo.Value; } }
    
    private bool OnCooldown
    {
        get
        {
            return CurrentCooldown < CooldownTime;
        }
    }

    private WeaponInputResponse _previousResponse = default(WeaponInputResponse);
    private float? _fireDownTime;

    public WeaponInputResponse PollInput()
    {
        WeaponInputResponse response = WeaponInputResponse.Create(_previousResponse, gameObject);

        if (response.FireButtonDown && _fireDownTime == null)
            _fireDownTime = Time.time;

        if (response.HasDirection)
        {
            _direction.Value = response.InputDirection;
        }

        _weaponCanFire.Value = CanFire();

        _previousResponse = response;

        return response;
    }
    public void ToggleFireDown(WeaponInputResponse response)
    {
        if (response.FireButtonDown && _fireDownTime == null)
        {
            _fireDownTime = Time.time;
        }
        else
        {
            _fireDownTime = null;
        }
    }
    private bool CanFire()
    {
        return !OnCooldown && CurrentAmmo > 0;
    }
}