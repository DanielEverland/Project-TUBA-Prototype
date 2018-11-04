using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Weapon",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 0)]
public class WeaponCollection : Collection<Weapon>
{
}
