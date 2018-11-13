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
    private Image _segmentElement;
    [SerializeField]
    private Transform _layoutParent;

    protected Transform LayoutParent => _layoutParent;
    protected Image SegmentElement => _segmentElement;
    protected Image[] Segments { get; set; }
    protected float HealthPerSegment => _healthPerSegment.Value;
    protected float CurrentHealth => _currentHealth.Value;
    protected float MaxHealth => _maxHealth.Value;
    protected int SegmentCount => _segmentCount.Value;

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
    }
    protected virtual void Update()
    {
        DrawSegments();
    }
    protected virtual void DrawSegments()
    {
        for (int i = 0; i < Segments.Length; i++)
        {
            float minValue = HealthPerSegment * i;
            float maxValue = HealthPerSegment * (i + 1);
            float percentageValue = Mathf.Clamp01((CurrentHealth - minValue) / (maxValue - minValue));

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
