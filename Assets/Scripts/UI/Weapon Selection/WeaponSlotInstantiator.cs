using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotInstantiator : MonoBehaviour {

    [SerializeField]
    private IntReference _weaponSlotCount;
    [SerializeField]
    private WeaponSlotElement _weaponSlotPrefab;
    [SerializeField]
    private Transform _parent;

    private void Awake()
    {
        for (int i = 0; i < _weaponSlotCount.Value; i++)
        {
            WeaponSlotElement instance = Instantiate(_weaponSlotPrefab);
            instance.transform.SetParent(_parent);

            instance.Initialize(i);
        }
    }
}
