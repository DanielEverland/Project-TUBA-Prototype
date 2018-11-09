using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : ObjectMover {

    [SerializeField]
    private MeshRenderer _renderer;

    protected MeshRenderer Renderer => _renderer;
    protected float Damage => _damage;

    private float _damage;

    public void Initialize(Weapon weapon)
    {
        _damage = weapon.TriggerData.Power;
        _renderer.material.color = weapon.SeekerData.Color;
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
        
        foreach (RaycastHit2D hit in hits)
        {
            Health healthComponent = hit.collider.gameObject.GetComponent<Health>();

            if (healthComponent != null)
                healthComponent.TakeDamage(Damage);
        }

        if(hits.Length > 0)
        {
            Destroy(gameObject);
        }
    }
}
