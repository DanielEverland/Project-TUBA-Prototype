using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHandler : MonoBehaviour {

    [SerializeField]
    private IntVariable _slotAmount;
    
    public IEnumerable<Weapon> Weapons { get { return _weapons; } }

    private List<Weapon> _weapons;

    private void Awake()
    {
        _weapons = new List<Weapon>();

        for (int i = 0; i < _slotAmount.Value; i++)
        {
            _weapons.Add(new Weapon());
        }
    }
    public void ChangeTrigger(int index, TriggerData trigger)
    {
        _weapons[index].TriggerData = trigger;

        //_weaponsChanged.Raise();
    }
}
