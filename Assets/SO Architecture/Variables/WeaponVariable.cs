using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Other/Weapon",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER)]

public sealed class WeaponVariable : BaseVariable<Weapon>
{

}
