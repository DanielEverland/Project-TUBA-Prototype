using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSelector : MonoBehaviour {

    [SerializeField]
    private Dropdown _dropDown;

    private WeaponSlotElement _owner;

    public void Initialize(WeaponSlotElement owner)
    {
        _owner = owner;
        
        _dropDown.options = new List<Dropdown.OptionData>(PartLoader.TriggerData.Select(x => new Dropdown.OptionData(x.name)));
        _dropDown.onValueChanged.AddListener(OnSelectionChanged);
    }
    private void OnSelectionChanged(int index)
    {
        _owner.ChangeTrigger(PartLoader.TriggerData.ElementAt(index));
    }
}
