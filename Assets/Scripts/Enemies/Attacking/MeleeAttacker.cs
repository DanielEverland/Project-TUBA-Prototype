using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour {

    [SerializeField]
    private GameObjectReference _target;
    [SerializeField]
    private FloatReference _maxDistance;
    [SerializeField]
    private FloatReference _attackPower;
    [SerializeField]
    private FloatReference _cooldownTime;
    [SerializeField]
    private FloatReference _attackForce;
    [SerializeField]
    private CharacterController2D _characterController;
    [SerializeField]
    private CharacterController2DVariable _playerCharacterController;
    [SerializeField]
    private HealthVariable _playerHealth;

    protected GameObject Target => _target.Value;
    protected Vector2 TargetDirection => (Target.transform.position - transform.position).normalized;
    protected CharacterController2D PlayerCharacterController => _playerCharacterController.Value;
    protected Health PlayerHealth => _playerHealth.Value;

    protected float AttackPower => _attackPower.Value;
    protected float AttackForce => _attackForce.Value;
    protected float MaxDistance => _maxDistance.Value;
    protected float DistanceToTarget => Vector3.Distance(transform.position, Target.transform.position);
    protected bool IsWithinRange => DistanceToTarget <= MaxDistance;
    protected float Cooldown => _cooldownTime.Value;
    protected float LastAttackTime { get; set; } = float.MinValue;
    protected float TimeSinceLastAttack => Time.time - LastAttackTime;
    protected bool CanAttack => TimeSinceLastAttack > Cooldown;
    protected bool HasDamaged { get; set; }

    protected virtual void Update()
    {
        if (!IsWithinRange)
            return;

        if (CanAttack)
        {
            Attack();
        }
    }
    protected virtual void Attack()
    {
        HasDamaged = false;
        LastAttackTime = Time.time;

        JumpTowardsTarget();
    }
    protected virtual void JumpTowardsTarget()
    {
        _characterController.AddForce(TargetDirection * AttackForce);
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject == Target)
        {
            DamagePlayer();
        }
    }
    protected virtual void DamagePlayer()
    {
        if (HasDamaged)
            return;
        
        HasDamaged = true;

        PlayerCharacterController.AddForce(TargetDirection * AttackForce * 2);
        PlayerHealth.TakeDamage(AttackPower);
    }
}
