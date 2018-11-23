using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICondition : AIStateMachineObject
{
    public abstract bool Evaluate(Agent agent);
}
