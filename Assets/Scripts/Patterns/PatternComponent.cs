using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PatternComponent : ScriptableObject
{
    public virtual void CreateChildren(GameObject parent, GameObject prefab, ref PatternObject pattern) { }
}
