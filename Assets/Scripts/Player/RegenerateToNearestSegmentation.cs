using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateToNearestSegmentation : MonoBehaviour
{
    [SerializeField]
    private FloatReference _currentValue;
    [SerializeField]
    private FloatReference _valuePerSegmentation;
    [SerializeField]
    private GameEvent _onRegenerate;

    protected float CurrentValue { get => _currentValue.Value; set => _currentValue.Value = value; }
    protected float ValuePerSegmentation => _valuePerSegmentation.Value;
    protected GameEvent OnRegenerate => _onRegenerate;
    
    public void Regenerate()
    {
        float targetValue = Utility.GetMaxValueForSegment(CurrentValue, ValuePerSegmentation);
        
        // If we're already at segment max, we want to get to the next segment max
        if (targetValue == CurrentValue)
            targetValue += ValuePerSegmentation;

        CurrentValue = targetValue;
        _onRegenerate?.Raise();
    }
}
