using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour {

    [SerializeField]
    private FloatReference _duration;
    [SerializeField]
    private AnimationCurve _curve = AnimationCurve.Constant(0, 1, 1);

    protected float FullDuration => _duration.Value;
    protected float SlowdownStart { get; set; }
    protected AnimationCurve Curve => _curve;
       
    public virtual void Slowdown()
    {
        SlowdownStart = Time.unscaledTime;
    }
    protected virtual void Update()
    {
        float timeSinceSlowdown = Time.unscaledTime - SlowdownStart;

        if(timeSinceSlowdown < FullDuration)
        {
            float durationPercentage = timeSinceSlowdown / FullDuration;

            Time.timeScale = Curve.Evaluate(durationPercentage);
        }
    }
}
