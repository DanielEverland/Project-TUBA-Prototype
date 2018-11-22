using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionIsStateDone : AICondition
{
    [SerializeField]
    private AIState _targetState;

    protected AIState TargetState => _targetState;

    public override bool IsConditionsMet => TargetState.IsDone;
}
