using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICondition : AIStateMachineObject
{
    public abstract bool IsConditionsMet { get; }

    public static implicit operator bool(AICondition condition)
    {
        return condition.IsConditionsMet;
    }
}
