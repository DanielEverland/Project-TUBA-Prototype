using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdividerElement : MonoBehaviour {

    [SerializeField]
    private IntReference _maxSubdivides;
    [SerializeField]
    private AnimationCurve _sizeProgression = AnimationCurve.Linear(0, 1, 1, 0.3f);
    [SerializeField]
    private Transform _sizeTarget;

    public int MaxLevel => _maxSubdivides.Value;
    public int CurrentLevel { get; private set; }
    public string Name { get; private set; }

    protected float CurrentLevelPercentage => (float)CurrentLevel / (float)MaxLevel;
    protected AnimationCurve SizeProgressionCurve => _sizeProgression;

    protected virtual void Awake()
    {
        Name = name;        
    }
    public virtual void Initialize(int subDivideLevel, string name)
    {
        CurrentLevel = subDivideLevel;
        Name = name;

        SetSize();
        SetName(subDivideLevel);
    }
    protected virtual void SetName(int level)
    {
        gameObject.name = $"{Name} ({level})";
    }
    protected virtual void SetSize()
    {
        float multiplier = SizeProgressionCurve.Evaluate(CurrentLevelPercentage);
        _sizeTarget.localScale *= multiplier;
    }
    private void OnValidate()
    {
        if (_sizeTarget == null)
            _sizeTarget = GetComponent<Transform>();
    }
}
