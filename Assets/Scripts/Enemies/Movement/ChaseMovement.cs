using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMovement : EnemyMovementBase
{
    [SerializeField]
    private GameObjectReference _target;    
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private FloatReference _minDistance;

    protected virtual Color TargetLineColor => new Color(1, 0, 0, 0.1f);

    protected GameObject Target => _target.Value;    
    protected CharacterController CharacterController => _characterController;
    protected Vector3 Direction => (Target.transform.position - transform.position).normalized;
    protected float DistanceToTarget => Vector3.Distance(transform.position, Target.transform.position);
    protected float MinDistance => _minDistance.Value;
    protected bool IsWithinRange => DistanceToTarget <= MinDistance;

    protected virtual void Update()
    {
        Move(Direction, MovementSpeed);
        DrawDebug();
    }    
    protected virtual void Move(Vector3 direction, float speed) => CharacterController.Move(direction * speed);
    protected virtual void DrawDebug() => Debug.DrawLine(transform.position, Target.transform.position, TargetLineColor);
}
