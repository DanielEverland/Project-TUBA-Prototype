using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "WeaponGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Custom/Weapon",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 0)]
public sealed class WeaponGameEvent : GameEventBase<Weapon>
{
}