using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour {

    [SerializeField]
    private FloatReference _damageAmount = new FloatReference(10);

    protected float DamageAmount => _damageAmount.Value;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponentInChildren<Health>()?.TakeDamage(DamageAmount);
    }
}
