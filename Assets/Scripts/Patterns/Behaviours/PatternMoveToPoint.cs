using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMoveToPoint : PatternBehaviour
{
    [SerializeField]
    private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private FloatReference _animationTime = new FloatReference(5);
    
    public AnimationCurve Curve => _curve;
    public float AnimationTime => _animationTime.Value;
    
    public override void Update()
    {
        float time = Mathf.Clamp01(Time.time - StartTime / AnimationTime);
        float animationValue = Curve.Evaluate(time);

        Evaluate(x =>
        {
            x.transform.localPosition = Vector3.MoveTowards(x.transform.localPosition, Vector3.zero, animationValue);
        });
    }
}
