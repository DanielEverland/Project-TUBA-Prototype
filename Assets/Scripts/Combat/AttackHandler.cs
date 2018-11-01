using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandlerComponent
{
    void Poll(WeaponInputResponse input);
}
public class AttackHandler : MonoBehaviour {

    [SerializeField]
    private BoolReference _isFireButtonPressed;
    [SerializeField]
    private BoolReference _canFire;
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
    [SerializeField]
    private WeaponReloader _weaponReloader;

    private bool IsFireButtonPressed { get { return _isFireButtonPressed.Value; } }
    private bool IsFullyCharged { get { return _weaponCharger.IsFullyCharged; } }
    private bool CanFire { get { return _canFire.Value; } }
        
    private void Update()
    {
        WeaponInputResponse response = _weaponInputHandler.PollInput();
                
        if(!_weaponReloader.PollReload(response))
        {
            PollWeaponFire(response);
            _weaponInputHandler.ToggleFireDown(response);
        }

        _weaponAngler.Poll(response);
    }
    private void PollWeaponFire(WeaponInputResponse response)
    {
        if (IsFullyCharged && CanFire && IsFireButtonPressed)
        {
            _weaponFireHandler.Fire();
        }

        _weaponCooldownHandler.CalculateCooldown();
        _weaponCharger.Poll(response);
    }
}
