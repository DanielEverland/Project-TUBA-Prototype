using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMoveToPoint : PatternBehaviour
{
    [SerializeField]
    private GameObjectReference _target;
    [SerializeField]
    private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private FloatReference _animationTime = new FloatReference(5);

    public GameObject Target => _target.Value;
    public AnimationCurve Curve => _curve;
    public float AnimationTime => _animationTime.Value;
    
    public override void Update()
    {
        float time = Mathf.Clamp01(Time.time - StartTime / AnimationTime);
        float animationValue = Curve.Evaluate(time);

        Evaluate(x =>
        {
            Vector3.MoveTowards(x.transform.position, Vector3.zero, animationValue);
        });
    }
}
