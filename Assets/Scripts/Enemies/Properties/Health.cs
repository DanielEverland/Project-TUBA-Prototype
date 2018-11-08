using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    [SerializeField]
    private FloatReference _startHealth;
    [SerializeField]
    private FloatReference _health;
    [SerializeField]
    private bool _destroyBelowZero = true;
    [SerializeField]
    private GameObject _destroyTarget;
    [SerializeField]
    private HealthPostProcessor _postProcessor;

    [Space()]

    [SerializeField]
    private UnityEvent _onDamagedEvent;
    [SerializeField]
    private UnityEvent _onDeathEvent;
    
    public float CurrentHealth { get => _health.Value; set => _health.Value = value; }
    public bool DestroyBelowZero { get => _destroyBelowZero; set => _destroyBelowZero = value; }
    public float StartHealth => GetStartHealth();
    
    protected UnityEvent OnDamagedEvent => _onDamagedEvent;
    protected UnityEvent OnDeathEvent => _onDeathEvent;
    protected GameObject DestroyTarget => _destroyTarget;

    private bool _isDying = false;

    protected virtual void Start()
    {
        CurrentHealth = StartHealth;
    }
    public virtual void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        OnDamagedEvent.Invoke();

        if (CurrentHealth <= 0 && DestroyBelowZero)
        {
            Die();
        }            
    }
    public virtual void Die()
    {
        if (_isDying)
            return;

        _isDying = true;

        OnDeathEvent.Invoke();

        Destroy(DestroyTarget);
    }
    protected virtual void OnValidate()
    {
        if (_destroyTarget == null)
        {
            _destroyTarget = transform.parent == null ? gameObject : transform.parent.gameObject;
        }            
    }
    private float GetStartHealth() => _postProcessor == null ? _startHealth.Value : _postProcessor.ProcessMaxHealth(_startHealth.Value);
}
