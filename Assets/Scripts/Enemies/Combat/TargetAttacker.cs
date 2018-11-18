using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetAttacker : AIAttacker
{
    [SerializeField]
    private GameObjectVariable _target;
    [SerializeField]
    private FloatReference _attackInterval;

    protected GameObject Target => _target.Value;
    protected Vector2 TargetPosition { get; set; }
    protected Vector2 TargetDirection => (TargetPosition - (Vector2)transform.position).normalized;
    protected float AttackInterval => _attackInterval.Value;
    protected float TimeSinceLastAttack { get; set; }

    public override void Think()
    {
        base.Think();

        TargetPosition = Target.transform.position;
    }
    protected virtual void Update()
    {
        TimeSinceLastAttack += Time.deltaTime;

        if(TimeSinceLastAttack > AttackInterval && CanAttack)
        {
            TimeSinceLastAttack -= AttackInterval;

            Attack();
        }
    }
    public override void Reset()
    {
        base.Reset();

        TimeSinceLastAttack = 0;
    }
}
