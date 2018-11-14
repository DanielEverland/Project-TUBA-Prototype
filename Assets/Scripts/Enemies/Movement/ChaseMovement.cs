using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMovement : EnemyMovementBase
{
    [SerializeField]
    private GameObjectReference _target;
    [SerializeField]
    private FloatReference _minDistance = new FloatReference(0);

    protected virtual Color TargetLineColor => new Color(1, 0, 0, 0.1f);

    protected GameObject Target => _target.Value;
    protected float DistanceToTarget => Vector3.Distance(transform.position, TargetPosition);
    protected float MinDistance => _minDistance.Value;
    protected bool IsWithinRange => DistanceToTarget <= MinDistance;
    protected Vector3 TargetPosition { get; set; }
    protected Vector3 Direction { get; set; }

    public override void Think()
    {
        TargetPosition = Target.transform.position;
        Direction = (Target.transform.position - transform.position).normalized;
    }
    protected virtual void Update()
    {
        Move(Direction, MovementSpeed * Time.deltaTime);
        DrawDebug();
    }    
    protected virtual void Move(Vector3 direction, float speed) => CharacterController.Move(direction * speed);
    protected virtual void DrawDebug() => Debug.DrawLine(transform.position, Target.transform.position, TargetLineColor);
}
