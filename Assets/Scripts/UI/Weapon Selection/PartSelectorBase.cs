using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class PartSelectorBase : MonoBehaviour
{
    [SerializeField]
    private Dropdown _dropDown;

    private WeaponSlotElement _owner;

    protected abstract IEnumerable<PartBase> AvailableParts { get; }
    
    public void Initialize(WeaponSlotElement owner)
    {
        _owner = owner;

        _dropDown.options = new List<Dropdown.OptionData>(AvailableParts.Select(x => new Dropdown.OptionData(x.name)));
        _dropDown.onValueChanged.AddListener(OnSelectionChanged);
    }
    private void OnSelectionChanged(int index)
    {
        _owner.ChangePart(AvailableParts.ElementAt(index));
    }
}