using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SubdividerElement))]
public class SubdivideMovementPostProcessor : MovementPostProcessor {

    [SerializeField]
    private SubdividerElement _subdividerElement;
    [SerializeField]
    private AnimationCurve _movementCurve = AnimationCurve.Linear(0, 0.3f, 1, 1);

    protected int CurrentLevel => _subdividerElement.CurrentLevel;
    protected int MaxLevel => _subdividerElement.MaxLevel;
    protected float CurrentLevelPercentage => (float)CurrentLevel / (float)MaxLevel;

    public override float ProcessMovementSpeed(float movementSpeed)
    {
        return _movementCurve.Evaluate(CurrentLevelPercentage) * movementSpeed;
    }
    protected virtual void OnValidate()
    {
        if (_subdividerElement == null)
            _subdividerElement = GetComponent<SubdividerElement>();
    }
}
