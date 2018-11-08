using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private FloatReference _startHealth;
    [SerializeField]
    private FloatReference _health;
    [SerializeField]
    private GameEvent _onDamagedEvent;
    [SerializeField]
    private bool _destroyBelowZero = true;
    [SerializeField]
    private GameObject _destroyTarget;

    public System.Action OnDamaged { get; }
    public float CurrentHealth { get => _health.Value; set => _health.Value = value; }
    public bool DestroyBelowZero { get => _destroyBelowZero; set => _destroyBelowZero = value; }
    public float StartHealth => _startHealth.Value;
    
    protected GameEvent OnDamagedEvent => _onDamagedEvent;
    protected GameObject DestroyTarget => _destroyTarget;

    protected virtual void Awake()
    {
        CurrentHealth = StartHealth;
    }
    public virtual void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        OnDamagedEvent?.Raise();
        OnDamaged?.Invoke();

        if (CurrentHealth <= 0 && DestroyBelowZero)
            Destroy(DestroyTarget);
    }
    protected virtual void OnValidate()
    {
        if (_destroyTarget == null)
        {
            _destroyTarget = transform.parent == null ? gameObject : transform.parent.gameObject;
        }            
    }
}
