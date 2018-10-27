using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBase : ScriptableObject {

    public const string MENU_ROOT = "Parts/";
    public const int MENU_ORDER = 105;

    public virtual void OnEquipped() { }
    public virtual void OnUneqipped() { }
}
