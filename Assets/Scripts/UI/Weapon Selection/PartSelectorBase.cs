using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class PartSelectorBase : MonoBehaviour
{
    [SerializeField]
    private Dropdown _dropDown;

    protected WeaponSlotElement Owner;

    protected abstract void OnSelectionChanged(int index);

    public void Initialize(WeaponSlotElement owner)
    {
        Owner = owner;

        _dropDown.options = new List<Dropdown.OptionData>(PartLoader.TriggerData.Select(x => new Dropdown.OptionData(x.name)));
        _dropDown.onValueChanged.AddListener(OnSelectionChanged);
    }
}