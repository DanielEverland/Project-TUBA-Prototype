using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponContainer.asset",
    menuName = MENU_ROOT + "Weapon Container",
    order = MENU_ORDER)]
public class WeaponContainer : InventoryBase
{
    [SerializeField]
    private IntReference _slotAmount;
    [SerializeField]
    private GameEvent _weaponUpdated;

    public Weapon this[int index]
    {
        get
        {
            return Weapons[index];
        }
        set
        {
            Weapons[index] = value;
        }
    }

    private ItemCollection<Weapon> Weapons
    {
        get
        {
            if (_weapons == null)
            {
                CreateNewCollection();
            }                

            return _weapons;
        }
    }
    private ItemCollection<Weapon> _weapons;

    public void ChangePart(int index, PartBase data)
    {
        if(data is TriggerData)
        {
            ChangeTrigger(index, data as TriggerData);
        }
        else if(data is EventData)
        {
            ChangeEvent(index, data as EventData);
        }
        else if(data is SeekerData)
        {
            ChangeSeeker(index, data as SeekerData);
        }
        else
        {
            throw new System.NotImplementedException();
        }

        OnWeaponsUpdated();
    }
    private void ChangeTrigger(int index, TriggerData data)
    {
        Weapons[index].TriggerData = data;
    }
    private void ChangeEvent(int index, EventData data)
    {
        Weapons[index].EventData = data;
    }
    private void ChangeSeeker(int index, SeekerData data)
    {
        Weapons[index].SeekerData = data;
    }
    private void OnWeaponsUpdated()
    {
        _weaponUpdated.Raise();
    }
    private void CreateNewCollection()
    {
        if (_slotAmount == null)
            throw new System.NullReferenceException();

        _weapons = new ItemCollection<Weapon>(_slotAmount.Value);

        for (int i = 0; i < _slotAmount.Value; i++)
        {
            _weapons[i] = new Weapon();
        }
    }
}