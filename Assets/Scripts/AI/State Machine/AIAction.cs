using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : AIStateMachineObject
{
    public virtual void Think(Agent agent) { }
    public virtual void PerformAction(Agent agent) { }
}
