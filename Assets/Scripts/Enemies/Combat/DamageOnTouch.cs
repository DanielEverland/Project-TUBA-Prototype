using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour {

    [SerializeField]
    private FloatReference _damageAmount = new FloatReference(10);
    [SerializeField]
    private LayerMask _ignorelayer;
    [SerializeField]
    private FloatReference _radius = new FloatReference(0.5f);
    [SerializeField]
    private BoolReference _destroyOnHit = new BoolReference(false);
    [SerializeField]
    private GameObject _destroyTarget;

    protected GameObject DestroyTarget => _destroyTarget;
    protected float Radius => _radius.Value;
    protected float DamageAmount => _damageAmount.Value;
    protected int LayerMask => _ignorelayer.value;
    protected bool DestroyOnHit => _destroyOnHit.Value;

    protected virtual void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, Radius, Vector2.zero);

        if(hit)
        {
            if (LayerMask != (LayerMask | 1 << hit.collider.gameObject.layer))
                Hit(hit.collider.gameObject);
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask != (LayerMask | 1 << collision.gameObject.layer))
            Hit(collision.gameObject);
    }
    protected virtual void Hit(GameObject target)
    {
        Health health = target.GetComponentInChildren<Health>();
        
        if(health != null)
        {
            if (!health.IsInvincible)
                Destroy();

            health.TakeDamage(DamageAmount);
        }
        else
        {
            Destroy();
        }
    }
    protected virtual void Destroy()
    {
        if (DestroyOnHit && DestroyTarget != null)
            Destroy(DestroyTarget);
    }
    protected virtual void OnValidate()
    {
        if (DestroyTarget == null)
            _destroyTarget = gameObject;
    }
}
