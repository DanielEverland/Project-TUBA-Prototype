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

    protected Dictionary<GameObject, Vector2> StartPositions { get; set; }

    public override void Initialize()
    {
        base.Initialize();
        
        StartPositions = new Dictionary<GameObject, Vector2>();
        Evaluate(x => StartPositions.Add(x, x.transform.localPosition));
    }
    public override void Update()
    {
        float time = Mathf.Clamp01((Time.time - StartTime) / AnimationTime);
        float animationValue = Curve.Evaluate(time);
        
        Evaluate(x =>
        {
            if (x != null)
                x.transform.localPosition = Vector3.Lerp(StartPositions[x], Vector3.zero, animationValue);
        });

        if (time == 1)
            Destroy(Parent);            
    }
}
