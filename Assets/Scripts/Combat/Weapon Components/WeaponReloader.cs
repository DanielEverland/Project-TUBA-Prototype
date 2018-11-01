using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour
{
    [SerializeField]
    private WeaponVariable _selectedWeapon;
    [SerializeField]
    private GameEvent _onAmmoReloaded;
    [SerializeField]
    private IntReference _maxAmmoProperty;
    [SerializeField]
    private FloatReference _reloadTimeProperty;
    [SerializeField]
    private BoolReference _isReloading;
    [SerializeField]
    private IntReference _currentAmmo;

    private bool IsReloading { get { return _isReloading.Value; } set { _isReloading.Value = value; } }
    private int MaxAmmoProperty { get { return _maxAmmoProperty.Value; } }
    private float ReloadTimeProperty { get { return _reloadTimeProperty.Value; } }

    public int CurrentAmmo
    {
        get
        {
            return _selectedWeapon.Value.CurrentAmmo;
        }
        set
        {
            _selectedWeapon.Value.CurrentAmmo = value;
            _currentAmmo.Value = _selectedWeapon.Value.CurrentAmmo;
        }
    }

    private float _reloadTimePassed;

    /// <summary>
    /// Returns true if reloading is in progress or started
    /// </summary>
    public bool PollReload(WeaponInputResponse response)
    {
        if (IsReloading)
            return true;

        if (response.ReloadButtonDown && CurrentAmmo != MaxAmmoProperty)
        {
            OnStartReload();
            return true;
        }

        return false;
    }
    private void Update()
    {
        if (IsReloading)
            DoReload();
    }
    private void DoReload()
    {
        _reloadTimePassed += Time.deltaTime;

        if (_reloadTimePassed > ReloadTimeProperty)
        {
            OnAmmoReloaded();
            _reloadTimePassed -= ReloadTimeProperty;
        }

        if (CurrentAmmo == MaxAmmoProperty)
            OnReloadStopped();
    }
    private void OnAmmoReloaded()
    {
        CurrentAmmo = Mathf.Clamp(CurrentAmmo + 1, 0, MaxAmmoProperty);

        _onAmmoReloaded.Raise();
    }
    private void OnReloadStopped()
    {
        IsReloading = false;
    }
    private void OnStartReload()
    {
        IsReloading = true;
        _reloadTimePassed = 0;
    }
}