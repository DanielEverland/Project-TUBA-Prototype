using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdividerMeleeAttackerPostProcessor : MeleeAttackerPostProcessor
{
    [SerializeField]
    private SubdividerElement _subdividerElement;
    [SerializeField]
    private AnimationCurve _attackForceCurve = AnimationCurve.Linear(0, 1, 1, 0.2f);

    protected int CurrentLevel => _subdividerElement.CurrentLevel;
    protected int MaxLevel => _subdividerElement.MaxLevel;
    protected float CurrentLevelPercentage => (float)CurrentLevel / (float)MaxLevel;

    public override float ProcessAttackForce(float value)
    {
        return _attackForceCurve.Evaluate(CurrentLevelPercentage) * value;
    }
    protected virtual void OnValidate()
    {
        if (_subdividerElement == null)
            _subdividerElement = GetComponent<SubdividerElement>();
    }
}
