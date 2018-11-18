using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAttacker : AIComponent
{
    public bool CanAttack { get; set; } = false;
    
    protected virtual void Awake()
    {
        Reset();
    }
    public virtual void Initialize()
    {
        Reset();
    }
    public override void Think() { }
    public virtual void Attack() { }
    public virtual void Reset() { }
}
