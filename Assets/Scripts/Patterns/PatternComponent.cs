using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PatternComponent : ScriptableObject
{
    [SerializeField]
    private GameObject _prefab;

    public GameObject Prefab { get => _prefab; set => _prefab = value; }

    public virtual void CreateChildren(GameObject parent, ref PatternObject pattern) { }
}
