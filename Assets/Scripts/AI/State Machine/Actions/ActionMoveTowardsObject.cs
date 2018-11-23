using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveTowardsObject : AIAction
{
    [SerializeField]
    private GameObjectReference _target;
    [SerializeField]
    private FloatReference _movementSpeed;

    protected GameObject Target => _target.Value;
    protected Vector2 TargetPosition { get; set; }
    protected float MovementSpeed => _movementSpeed.Value;

    public override void Think(Agent agent)
    {
        TargetPosition = Target.transform.position;
    }
    public override void PerformAction(Agent agent)
    {
        agent.CharacterController.Move(Direction(agent) * MovementSpeed * Time.deltaTime);
    }
    private Vector2 Direction(Agent agent)
    {
        return (TargetPosition - (Vector2)agent.transform.position).normalized;
    }
}
