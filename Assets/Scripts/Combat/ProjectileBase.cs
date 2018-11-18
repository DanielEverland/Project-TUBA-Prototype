using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : ObjectMover {

    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private LayerMask _ignoreLayer;

    protected MeshRenderer Renderer => _renderer;
    protected float Damage => _damage;

    private float _damage;

    public void Initialize(Weapon weapon)
    {
        _damage = weapon.TriggerData.Power;
        _renderer.material.color = weapon.SeekerData.Color;
    }
    public void Initialize(float damage, Color color)
    {
        _damage = damage;
        _renderer.material.color = color;
    }
    protected override void Update()
    {
        base.Update();

        PollCollision();
    }
    protected virtual void PollCollision()
    {
        Vector3 direction = GetDirection();
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction.normalized, Velocity * Time.deltaTime);
        bool hitAnything = false;

        foreach (RaycastHit2D hit in hits)
        {
            if(_ignoreLayer != (_ignoreLayer | 1 << hit.collider.gameObject.layer))
            {
                hitAnything = true;

                Health healthComponent = hit.collider.gameObject.GetComponent<Health>();

                if (healthComponent != null)
                {
                    healthComponent.TakeDamage(Damage);
                    break;
                }
            }
        }

        if(hitAnything)
        {
            Destroy(gameObject);
        }
    }
}
