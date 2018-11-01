using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandlerComponent
{
    void Poll(WeaponInputResponse input);
}
public class AttackHandler : MonoBehaviour {
    
    [SerializeField]
    private Transform _weaponTransform;    
    [SerializeField]
    private FloatReference _cooldownTime;
    [SerializeField]
    private FloatReference _currentCooldown;
    [SerializeField]
    private BoolReference _useCharge;    
    [SerializeField]
    private FloatReference _reloadTime;
    [SerializeField]
    private Vector3Reference _direction;
    [SerializeField]
    private GameEvent _onAmmoReloaded;
    [SerializeField]
    private Transform _weaponDirection;
    [SerializeField]
    private WeaponCharger _weaponCharger;
    [SerializeField]
    private WeaponFireHandler _weaponFireHandler;
    [SerializeField]
    private WeaponAngler _weaponAngler;
    [SerializeField]
    private BoolReference _weaponCanFire;

    private bool IsFullyCharged { get { return _weaponCharger.IsFullyCharged; } }

    private bool UseCharge { get { return _useCharge.Value; } }
    private float CurrentCooldown { get { return _currentCooldown.Value; } set { _currentCooldown.Value = value; } }    
    private float CooldownTime { get { return _cooldownTime.Value; } }
    

    private bool OnCooldown
    {
        get
        {
            return CurrentCooldown < CooldownTime;
        }
    }    

    private bool _isReloading = false;
    private float _reloadTimePassed;
    
    private float? _fireDownTime = null;
    private WeaponInputResponse _previousResponse = default(WeaponInputResponse);
    
    private void Update()
    {
        WeaponInputResponse response = PollInput();
                
        if(ShouldReload(response) || _isReloading)
        {
            DoReload();
            return;
        }
        else
        {
            PollWeaponFire(response);
            ToggleFireDown(response);
        }

        _weaponAngler.Poll(response);

        _previousResponse = response;
    }
    private void DoReload()
    {
        _reloadTimePassed += Time.deltaTime;

        if(_reloadTimePassed > _reloadTime.Value)
        {
            OnAmmoReloaded();
            _reloadTimePassed -= _reloadTime.Value;
        }

        //if (CurrentAmmo == _maxAmmoCount.Value)
        //    OnReloadStopped();
    }
    private bool ShouldReload(WeaponInputResponse response)
    {
        //if (response.ReloadButtonDown && CurrentAmmo != _maxAmmoCount.Value)
        //{
        //    OnStartReload();
        //    return true;
        //}            

        return false;
    }
    private void OnAmmoReloaded()
    {
        //CurrentAmmo = Mathf.Clamp(CurrentAmmo + 1, 0, _maxAmmoCount.Value);       

        _onAmmoReloaded.Raise();
    }
    private void OnReloadStopped()
    {
        _isReloading = false;
    }
    private void OnStartReload()
    {
        _isReloading = true;
        _reloadTimePassed = 0;
    }
    private void PollWeaponFire(WeaponInputResponse response)
    {
        CalculateCooldown();
        PollFireWeapon(response);
        _weaponCharger.Poll(response);
    }    
    private void PollFireWeapon(WeaponInputResponse response)
    {        
        if(IsFullyCharged && CanFire() && IsFireButtonPressed(response))
        {
            _weaponFireHandler.Fire();
        }
    }
    private bool IsFireButtonPressed(WeaponInputResponse response)
    {
        if (UseCharge)
        {
            return response.FireButtonUp;
        }
        else
        {
            return response.FireButtonDown;
        }
    }
    private void CalculateCooldown()
    {
        CurrentCooldown = Mathf.Clamp(Time.time - _weaponFireHandler.WeaponLastFire, 0, CooldownTime);
    }
    private void ToggleFireDown(WeaponInputResponse response)
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
    private WeaponInputResponse PollInput()
    {
        WeaponInputResponse response = WeaponInputResponse.Create(_previousResponse, gameObject);

        if (response.FireButtonDown && _fireDownTime == null)
            _fireDownTime = Time.time;

        if (response.HasDirection)
        {
            _direction.Value = response.InputDirection;
        }

        _weaponCanFire.Value = CanFire();

        return response;
    }
    private bool CanFire()
    {
        return !OnCooldown; /*&& CurrentAmmo > 0;*/
    }
}
