using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternBehaviour : ScriptableObject
{
    public PatternObject Pattern { get; set; }

    protected float StartTime { get; set; }
    protected GameObject Parent => Pattern.Parent;
    
    public abstract void Update();

    public virtual void Initialize()
    {
        StartTime = Time.time;
    }
    protected void Evaluate(Action<GameObject> predicate)
    {
        for (int i = 0; i < Pattern.Children.Count; i++)
        {
            predicate(Pattern.Children[i]);
        }
    }
}
