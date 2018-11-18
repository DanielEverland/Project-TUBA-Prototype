using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternObject
{
    public List<GameObject> Children { get; set; } = new List<GameObject>();

    public void AddChild(GameObject child)
    {
        Children.Add(child);
    }
}