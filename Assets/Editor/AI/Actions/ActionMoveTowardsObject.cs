using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveTowardsObject : AIStateMachineAction
{
    [SerializeField]
    private GameObjectReference _target;

    protected GameObject Target => _target.Value;
    protected Vector2 TargetPosition { get; set; }
    protected Vector2 Direction => (TargetPosition - (Vector2)GameObject.transform.position).normalized;

    public override void Think()
    {
        TargetPosition = Target.transform.position;
    }
    public override void Update()
    {
        Agent.CharacterController.Move(Direction);
    }
}
