using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionObjectWithinRange : AICondition
{
    [SerializeField]
    private GameObjectReference _targetObject;
    [SerializeField]
    private FloatReference _distance;

    protected GameObject Target => _targetObject.Value;
    protected float Distance => _distance.Value;
    
    public override bool Evaluate(Agent agent)
    {
        return Vector2.Distance(agent.transform.position, Target.transform.position) <= Distance;
    }
}
