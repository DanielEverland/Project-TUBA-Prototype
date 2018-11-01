using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandlerComponent
{
    void Poll(WeaponInputResponse input);
}
public class AttackHandler : MonoBehaviour {

    [SerializeField]
    private BoolReference _canFire;
    [SerializeField]
    private Transform _weaponTransform;
    [SerializeField]
    private BoolReference _useCharge;    
    [SerializeField]
    private FloatReference _reloadTime;
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
    private WeaponInputHandler _weaponInputHandler;
    [SerializeField]
    private WeaponCooldownHandler _weaponCooldownHandler;
    
    private bool IsFullyCharged { get { return _weaponCharger.IsFullyCharged; } }
    private bool CanFire { get { return _canFire.Value; } }

    private bool UseCharge { get { return _useCharge.Value; } }
    private bool _isReloading = false;
    private float _reloadTimePassed;
    
    private void Update()
    {
        WeaponInputResponse response = _weaponInputHandler.PollInput();
                
        if(ShouldReload(response) || _isReloading)
        {
            DoReload();
            return;
        }
        else
        {
            PollWeaponFire(response);
            _weaponInputHandler.ToggleFireDown(response);
        }

        _weaponAngler.Poll(response);

        
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
        PollFireWeapon(response);
        _weaponCooldownHandler.CalculateCooldown();
        _weaponCharger.Poll(response);
    }    
    private void PollFireWeapon(WeaponInputResponse response)
    {        
        if(IsFullyCharged && CanFire && IsFireButtonPressed(response))
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
}
