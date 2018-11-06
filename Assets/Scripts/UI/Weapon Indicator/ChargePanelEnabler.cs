using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePanelEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel = null;
    [SerializeField]
    private bool _defaultState = false;

    private void Awake()
    {
        Toggle(_defaultState);
    }
    public void PollPanel(Weapon currentWeapon) => Toggle(currentWeapon.TriggerData.UseCharge);
    private void Toggle(bool active) => _panel.SetActive(active);
}