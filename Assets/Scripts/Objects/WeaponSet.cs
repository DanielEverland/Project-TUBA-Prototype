using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponSet.asset",
    menuName = SOArchitecture_Utility.SETS_SUBMENU + "Weapon",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER)]
public class WeaponSet : Collection<Weapon>
{
}
