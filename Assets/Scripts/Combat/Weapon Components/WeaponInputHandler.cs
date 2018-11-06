using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInputHandler : MonoBehaviour
{
    [SerializeField]
    private BoolReference _useCharge;
    [SerializeField]
    private BoolReference _fireButtonPressed;
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

    private bool UseCharge => _useCharge.Value;
    private float CurrentCooldown => _currentCooldown.Value;
    private float CooldownTime => _cooldownTimeProperty.Value;
    private int CurrentAmmo => _currentAmmo.Value;

    private Vector3 Direction { get { return _direction.Value; } set { _direction.Value = value; } }
    private bool FireButtonPressed { get { return _fireButtonPressed.Value; } set { _fireButtonPressed.Value = value; } }
    private bool WeaponCanFire { get { return _weaponCanFire.Value; } set { _weaponCanFire.Value = value; } }
    
    private bool OnCooldown
    {
        get
        {
            return CurrentCooldown < CooldownTime;
        }
    }

    private WeaponInputResponse _previousResponse = default;
    private float? _fireDownTime;
    
    public WeaponInputResponse PollInput()
    {
        WeaponInputResponse input = WeaponInputResponse.Create(_previousResponse, gameObject);

        if (input.FireButtonDown && _fireDownTime == null)
            _fireDownTime = Time.time;

        if (input.HasDirection)
        {
            Direction = input.InputDirection;
        }

        WeaponCanFire = CanFire();
        FireButtonPressed = IsFireButtonPressed(input);

        _previousResponse = input;

        return input;
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
    private bool CanFire() => !OnCooldown && CurrentAmmo > 0;
    private bool IsFireButtonPressed(WeaponInputResponse input) => UseCharge ? input.FireButtonUp : input.FireButtonDown;
}