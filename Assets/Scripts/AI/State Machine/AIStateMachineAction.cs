using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateMachineAction : AIStateMachineObject
{
    public virtual void Think() { }
    public virtual void Update() { }
}
