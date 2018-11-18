using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternObject
{
    public PatternObject(GameObject parentObject)
    {
        _parent = parentObject;
    }

    public GameObject Parent => _parent;
    public List<GameObject> Children { get; set; } = new List<GameObject>();

    private readonly GameObject _parent;

    public void AddChild(GameObject child)
    {
        Children.Add(child);
    }

    public static implicit operator GameObject(PatternObject pattern)
    {
        return pattern.Parent;
    }
}