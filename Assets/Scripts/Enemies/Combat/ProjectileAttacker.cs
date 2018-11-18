using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttacker : TargetAttacker
{
    [SerializeField]
    private FloatReference _attackDamage;
    [SerializeField]
    private Color _color = Color.white;

    protected float AttackDamage => _attackDamage.Value;
    protected Color Color => _color;

	protected virtual void SpawnProjectile(ProjectileBase projectilePrefab)
    {
        ProjectileBase instance = Instantiate(projectilePrefab);
        instance.Initialize(AttackDamage, Color);

        float angle = Mathf.Atan2(TargetDirection.y, TargetDirection.x) * Mathf.Rad2Deg;

        instance.transform.position = transform.position;
        instance.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
