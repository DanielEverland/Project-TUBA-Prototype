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
    protected Vector2 Direction => (TargetPosition - (Vector2)GameObject.transform.position).normalized;
    protected float MovementSpeed => _movementSpeed.Value;

    public override void Think()
    {
        TargetPosition = Target.transform.position;
    }
    public override void Update()
    {
        Agent.CharacterController.Move(Direction * MovementSpeed * Time.deltaTime);
    }
}
