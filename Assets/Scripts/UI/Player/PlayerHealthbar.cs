using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour {

    [SerializeField]
    private FloatReference _currentHealth;
    [SerializeField]
    private FloatReference _maxHealth;
    [SerializeField]
    private FloatReference _healthPerSegment;
    [SerializeField]
    private IntReference _segmentCount;
    [SerializeField]
    private FloatReference _lerpSpeed = new FloatReference(10);
    [SerializeField]
    private BoolReference _lerpIncreases = new BoolReference(true);
    [SerializeField]
    private BoolReference _lerpDecreases = new BoolReference(true);
    [SerializeField]
    private Image _segmentElement;
    [SerializeField]
    private Transform _layoutParent;

    protected Transform LayoutParent => _layoutParent;
    protected Image SegmentElement => _segmentElement;
    protected Image[] Segments { get; set; }
    protected bool ShouldLerpIncreases => _lerpIncreases.Value;
    protected bool ShouldLerpDecreases => _lerpDecreases.Value;
    protected float LerpSpeed => _lerpSpeed.Value;
    protected float HealthPerSegment => _healthPerSegment.Value;
    protected float CurrentHealth => _currentHealth.Value;
    protected float MaxHealth => _maxHealth.Value;
    protected int SegmentCount => _segmentCount.Value;
    protected float LerpValue { get; set; }
    protected bool IsLerping { get; set; }
    protected float PreviousHealth { get; set; }

    public virtual Image this[int index]
    {
        get
        {
            return Segments[index];
        }
    }
    protected virtual void Awake()
    {
        SpawnSegments();
        PollDifferences();
    }
    protected virtual void Update()
    {
        PollDifferences();

        Lerp();
        DrawSegments();

        CacheHealth();
    }
    protected virtual void PollDifferences()
    {
        if(CurrentHealth != PreviousHealth)
        {
            IsLerping = false;

            if (CurrentHealth > PreviousHealth && ShouldLerpIncreases)
                IsLerping = true;

            if (CurrentHealth < PreviousHealth && ShouldLerpDecreases)
                IsLerping = true;
        }
    }
    protected virtual void Lerp()
    {
        LerpValue = Mathf.Lerp(LerpValue, CurrentHealth, LerpSpeed * Time.deltaTime);
    }
    protected virtual void CacheHealth()
    {
        PreviousHealth = CurrentHealth;
    }
    protected virtual void DrawSegments()
    {
        for (int i = 0; i < Segments.Length; i++)
        {
            float healthValue = IsLerping ? LerpValue : CurrentHealth;

            float minValue = HealthPerSegment * i;
            float maxValue = HealthPerSegment * (i + 1);
            float percentageValue = Mathf.Clamp01((healthValue - minValue) / (maxValue - minValue));

            this[i].fillAmount = percentageValue;
        }
    }
    protected virtual void SpawnSegments()
    {
        // Remove all existing segments, if any
        for (int i = 0; i < Segments?.Length; i++)
        {
            Destroy(this[i].gameObject);
        }
        
        Segments = new Image[SegmentCount];
        for (int i = 0; i < SegmentCount; i++)
        {
            Image image = Instantiate(SegmentElement);
            image.transform.SetParent(LayoutParent);

            Segments[i] = image;
        }
    }
}
