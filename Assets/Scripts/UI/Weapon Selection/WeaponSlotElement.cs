using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotElement : MonoBehaviour {

    [SerializeField]
    private List<PartSelectorBase> _triggerSelectors;
    [SerializeField]
    private WeaponContainer _weaponContainer;

#if UNITY_EDITOR
    [SerializeField]
    private int _debugWeaponIndex = 0;
#endif

    private int? _weaponIndex = null;
    
    private void Start()
    {
        for (int i = 0; i < _triggerSelectors.Count; i++)
        {
            _triggerSelectors[i].Initialize(this);
        }
    }
    public void Initialize(int weaponIndex)
    {
        _weaponIndex = weaponIndex;
    }
    public void ChangePart(PartBase partdata)
    {
#if UNITY_EDITOR
        if(_weaponIndex == null)
        {
            Debug.LogWarning("Not initialized, using debug weapon index");

            _weaponContainer.ChangePart(_debugWeaponIndex, partdata);
            return;
        }
#endif

        _weaponContainer.ChangePart(_weaponIndex.Value, partdata);
    }
}
