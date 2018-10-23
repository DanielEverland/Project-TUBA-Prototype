using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "WeaponGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Other/Weapon",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER)]
public sealed class WeaponGameEvent : GameEvent
{
    [SerializeField]
    private WeaponReference _value;

    public Weapon Value { get { return _value; } }
}
