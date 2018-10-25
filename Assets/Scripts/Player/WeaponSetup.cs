using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensures weapon objects are setup correctly and ready for use
/// </summary>
public class WeaponSetup : MonoBehaviour
{
    [SerializeField]
    private WeaponVariable _selectedWeapon;
    [SerializeField]
    private WeaponSet _weaponInventory;
    [SerializeField]
    private IntReference _weaponSlots;

    private void Awake()
    {
        _weaponInventory.Items.Clear();

        for (int i = 0; i < _weaponSlots.Value; i++)
        {
            Weapon weapon = Weapon.CreateInstance<Weapon>();
            weapon.AssignRandomParts();
            
            _weaponInventory.Add(weapon);
        }

        _selectedWeapon.Value = _weaponInventory[0];
    }
}