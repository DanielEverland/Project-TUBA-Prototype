using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttacker : ProjectileAttacker
{
    [SerializeField]
    private ProjectileBase _projectilePrefab;

    public override void Attack()
    {
        SpawnProjectile(_projectilePrefab);
    }
}
