using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternBehaviour : ScriptableObject
{
    protected PatternObject Pattern { get; private set; }
    protected float StartTime { get; set; }
    
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
