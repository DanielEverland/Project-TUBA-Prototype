using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionIsStateDone : AIStateMachineCondition
{
    [SerializeField]
    private AIStateMachineStateNode _targetState;

    protected AIStateMachineStateNode TargetState => _targetState;

    public override bool IsConditionsMet => TargetState.IsDone;
}
