using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour {

    [SerializeField]
    private GameObjectReference _target;
    [SerializeField]
    private FloatReference _maxDistance;
    [SerializeField]
    private FloatReference _damage;
    [SerializeField]
    private FloatReference _cooldownTime;
    [SerializeField]
    private FloatReference _force;
    [SerializeField]
    private CharacterController2D _characterController;
    [SerializeField]
    private CharacterController2DVariable _playerCharacterController;
    [SerializeField]
    private HealthVariable _playerHealth;
    [SerializeField, HideInInspector]
    private MeleeAttackerPostProcessor _postProcessor;

    protected GameObject Target => _target.Value;
    protected Vector2 TargetDirection => (Target.transform.position - transform.position).normalized;
    protected CharacterController2D PlayerCharacterController => _playerCharacterController.Value;
    protected Health PlayerHealth => _playerHealth.Value;

    protected float Damage => _damage.Value;
    protected float Force => GetAttackForce();
    protected float MaxDistance => _maxDistance.Value;
    protected float DistanceToTarget => Vector3.Distance(transform.position, Target.transform.position);
    protected bool IsWithinRange => DistanceToTarget <= MaxDistance;
    protected float Cooldown => _cooldownTime.Value;
    protected float LastAttackTime { get; set; } = float.MinValue;
    protected float TimeSinceLastAttack => Time.time - LastAttackTime;
    protected bool CanAttack => TimeSinceLastAttack > Cooldown;
    protected bool HasDamaged { get; set; }

    protected const float ATTACK_PUSH_MULTIPLIER = 2;

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
        _characterController.AddForce(TargetDirection * Force);
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

        PlayerCharacterController.AddForce(TargetDirection * Force * ATTACK_PUSH_MULTIPLIER);
        PlayerHealth.TakeDamage(Damage);
    }
    protected virtual float GetAttackForce()
    {
        return _postProcessor == null ? _force.Value : _postProcessor.ProcessAttackForce(_force.Value);
    }
    protected virtual void OnValidate()
    {
        _postProcessor = GetComponent<MeleeAttackerPostProcessor>();
    }
}
