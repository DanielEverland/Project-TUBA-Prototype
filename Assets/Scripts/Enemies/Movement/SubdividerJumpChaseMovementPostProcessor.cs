using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdividerJumpChaseMovementPostProcessor : JumpChaseMovementPostProcessor
{
    [SerializeField]
    private SubdividerElement _subdividerElement;
    [SerializeField]
    private AnimationCurve _pauseBetweenJumpCurve = AnimationCurve.Constant(0, 1, 1);

    protected int CurrentLevel => _subdividerElement.CurrentLevel;
    protected int MaxLevel => _subdividerElement.MaxLevel;
    protected float CurrentLevelPercentage => (float)CurrentLevel / (float)MaxLevel;

    public override float ProcessPauseBetweenJumps(float value)
    {
        return _pauseBetweenJumpCurve.Evaluate(CurrentLevelPercentage) * value;
    }
    protected virtual void OnValidate()
    {
        if (_subdividerElement == null)
            _subdividerElement = GetComponent<SubdividerElement>();
    }
}
