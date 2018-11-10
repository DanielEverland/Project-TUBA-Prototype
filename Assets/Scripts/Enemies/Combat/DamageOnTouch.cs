using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour {

    [SerializeField]
    private FloatReference _damageAmount = new FloatReference(10);
    [SerializeField]
    private LayerMask _ignorelayer;

    protected float DamageAmount => _damageAmount.Value;
    protected int LayerMask => _ignorelayer.value;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask != (LayerMask | 1 << collision.gameObject.layer))
            collision.gameObject.GetComponentInChildren<Health>()?.TakeDamage(DamageAmount);
    }
}
