using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawnerOnTarget : TargetAttacker
{
    [SerializeField]
    private Pattern _pattern;
    
    protected Pattern Pattern => _pattern;

    public override void Attack()
    {
        PatternObject obj = Pattern.Spawn();
        obj.Parent.transform.position = Target.transform.position;
    }
}
