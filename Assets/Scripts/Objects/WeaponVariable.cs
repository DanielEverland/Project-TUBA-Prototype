using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Custom/Weapon",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER_VARIABLES + 0)]
public sealed class WeaponVariable : BaseVariable<Weapon>
{
}