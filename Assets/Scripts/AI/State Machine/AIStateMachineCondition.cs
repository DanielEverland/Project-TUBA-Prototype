using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateMachineCondition : AIStateMachineObject
{
    public abstract bool IsConditionsMet { get; }

    public static implicit operator bool(AIStateMachineCondition condition)
    {
        return condition.IsConditionsMet;
    }
}
