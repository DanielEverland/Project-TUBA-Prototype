using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandlerComponent
{
    void Poll(CombatInputResponse input);
}
public class AttackHandler : MonoBehaviour {

    
    [SerializeField]
    private WeaponVariable _selectedWeapon;
    [SerializeField]
    private Transform _weaponTransform;    
    [SerializeField]
    private FloatReference _cooldownTime;
    [SerializeField]
    private FloatReference _currentCooldown;
    [SerializeField]
    private BoolReference _useCharge;
    [SerializeField]
    private IntReference _currentAmmoCount;
    [SerializeField]
    private IntReference _maxAmmoCount;
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
    private SeekerSpawner _seekerSpawner;
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

    private bool _isReloading = false;
    private float _reloadTimePassed;
    private float _lastFireTime = float.MinValue;
    private float? _fireDownTime = null;
    private CombatInputResponse _previousResponse = default(CombatInputResponse);
    
    private void Update()
    {
        CombatInputResponse response = PollInput();
                
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
                
        AngleWeapon(response);

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

        if (CurrentAmmo == _maxAmmoCount.Value)
            OnReloadStopped();
    }
    private bool ShouldReload(CombatInputResponse response)
    {
        if (response.ReloadButtonDown && CurrentAmmo != _maxAmmoCount.Value)
        {
            OnStartReload();
            return true;
        }            

        return false;
    }
    private void OnAmmoReloaded()
    {
        CurrentAmmo = Mathf.Clamp(CurrentAmmo + 1, 0, _maxAmmoCount.Value);       

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
    private void PollWeaponFire(CombatInputResponse response)
    {
        CalculateCooldown();
        PollFireWeapon(response);
        _weaponCharger.Poll(response);
    }    
    private void PollFireWeapon(CombatInputResponse response)
    {        
        if(IsFullyCharged && CanFire() && IsFireButtonPressed(response))
        {
            Fire();
        }
    }
    private bool IsFireButtonPressed(CombatInputResponse response)
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
        CurrentCooldown = Mathf.Clamp(Time.time - _lastFireTime, 0, CooldownTime);
    }
    private void ToggleFireDown(CombatInputResponse response)
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
    private CombatInputResponse PollInput()
    {
        CombatInputResponse response = CombatInputResponse.Create(_previousResponse, gameObject);

        if (response.FireButtonDown && _fireDownTime == null)
            _fireDownTime = Time.time;

        if (response.HasDirection)
        {
            _direction.Value = response.InputDirection;
        }

        _weaponCanFire.Value = CanFire();

        return response;
    }
    private void AngleWeapon(CombatInputResponse response)
    {
        Vector3 direction = _direction.Value.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _weaponTransform.transform.eulerAngles = new Vector3(0, 0, angle);
    }    
    private bool CanFire()
    {
        return !OnCooldown && CurrentAmmo > 0;
    }
    private void Fire()
    {
        _lastFireTime = Time.time;

        CurrentAmmo = Mathf.Clamp(CurrentAmmo - 1, 0, _maxAmmoCount.Value);

        foreach (Weapon.Muzzle muzzle in _selectedWeapon.Value.TriggerData.Muzzles)
        {
            _seekerSpawner.SpawnSeeker(muzzle);
        }
    }
}
