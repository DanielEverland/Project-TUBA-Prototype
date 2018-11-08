using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdividerHealthPostProcessor : HealthPostProcessor
{
    [SerializeField]
    private SubdividerElement _subdividerElement;
    [SerializeField]
    private AnimationCurve _healthMultiplier = AnimationCurve.Linear(0, 1, 1, 0.2f);

    protected int CurrentLevel => _subdividerElement.CurrentLevel;
    protected int MaxLevel => _subdividerElement.MaxLevel;
    protected float CurrentLevelPercentage => (float)CurrentLevel / (float)MaxLevel;

    public override float ProcessMaxHealth(float maxHealth)
    {
        return _healthMultiplier.Evaluate(CurrentLevelPercentage) * maxHealth;
    }
    protected virtual void OnValidate()
    {
        if (_subdividerElement == null)
            _subdividerElement = GetComponent<SubdividerElement>();
    }    
}
