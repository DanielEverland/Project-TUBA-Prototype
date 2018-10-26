using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField]
    private WeaponVariable _selectedWeapon = null;
    [SerializeField]
    private WeaponSet _weaponInventory = null;
    [SerializeField]
    private WeaponGameEvent _onWeaponSelectionUpdated = null;
    [SerializeField]
    private bool _useScrollWheel = true;
    [SerializeField]
    private bool _useAlphaNumericKeys = true;
    
    private void Update()
    {
        if (_useScrollWheel)
            PollScrollWheel();

        if (_useAlphaNumericKeys)
            PollKeys();
    }
    private void PollScrollWheel()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            SelectNext();
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            SelectPrevious();
        }
    }
    private void PollKeys()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1) && _weaponInventory.Count > 0)
        {
            SelectWeapon(0);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2) && _weaponInventory.Count > 1)
        {
            SelectWeapon(1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3) && _weaponInventory.Count > 2)
        {
            SelectWeapon(2);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4) && _weaponInventory.Count > 3)
        {
            SelectWeapon(3);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5) && _weaponInventory.Count > 4)
        {
            SelectWeapon(4);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6) && _weaponInventory.Count > 5)
        {
            SelectWeapon(5);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha7) && _weaponInventory.Count > 6)
        {
            SelectWeapon(6);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha8) && _weaponInventory.Count > 7)
        {
            SelectWeapon(7);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha9) && _weaponInventory.Count > 8)
        {
            SelectWeapon(8);
        }
    }
    private void SelectNext()
    {
        int index = (GetIndex() + 1).Wrap(0, _weaponInventory.Count - 1);
        
        SelectWeapon(index);
    }
    private void SelectPrevious()
    {
        int index = (GetIndex() - 1).Wrap(0, _weaponInventory.Count - 1);

        SelectWeapon(index);
    }
    private int GetIndex()
    {
        return _weaponInventory.Items.IndexOf(_selectedWeapon.Value);
    }
    private void SelectWeapon(int index)
    {
        SelectWeapon(_weaponInventory[index]);
    }
    private void SelectWeapon(Weapon newSelection)
    {
        if (_selectedWeapon.Value == newSelection)
            return;

        _selectedWeapon.Value = newSelection;

        _onWeaponSelectionUpdated.Raise(_selectedWeapon.Value);
    }
}