using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    [SerializeField]
    private FloatReference _startHealth;
    [SerializeField]
    private FloatReference _health;
    [SerializeField]
    private bool _destroyBelowZero = true;
    [SerializeField]
    private GameObject _destroyTarget;
    [SerializeField]
    private Renderer _renderer;

    [Space()]

    [SerializeField]
    private UnityEvent _onDamagedEvent;
    [SerializeField]
    private UnityEvent _onDeathEvent;

    [HideInInspector, SerializeField]
    private HealthPostProcessor _postProcessor;

    public float CurrentHealth { get => _health.Value; set => _health.Value = value; }
    public bool DestroyBelowZero { get => _destroyBelowZero; set => _destroyBelowZero = value; }
    public float StartHealth => GetStartHealth();
    
    protected UnityEvent OnDamagedEvent => _onDamagedEvent;
    protected UnityEvent OnDeathEvent => _onDeathEvent;
    protected GameObject DestroyTarget => _destroyTarget;
    protected Renderer Renderer => _renderer;
    protected bool IsDying { get; set; }
    protected float TimeTookDamage { get; set; }
    protected MaterialData CurrentColorScheme { get; set; }

    protected const float INDICATOR_TIME = 0.1f;
    protected readonly Color INDICATOR_COLOR = Color.red;

    protected virtual void Start()
    {
        CurrentHealth = StartHealth;
    }
    protected virtual void Update()
    {
        PollRenderer();
    }
    public virtual void TakeDamage(float damageAmount)
    {
        TimeTookDamage = Time.time;
        CurrentHealth -= damageAmount;
        
        OnDamagedEvent.Invoke();

        if (CurrentHealth <= 0 && DestroyBelowZero)
        {
            Die();
        }
        else
        {
            ColorRenderer();
        }
    }
    protected virtual void PollRenderer()
    {
        if(CurrentColorScheme != null)
        {
            if(Time.time - TimeTookDamage > INDICATOR_TIME)
            {
                CurrentColorScheme.Revert();
                CurrentColorScheme = null;
            }
        }
    }
    protected virtual void ColorRenderer()
    {
        if (CurrentColorScheme != null || Renderer == null)
            return;

        CurrentColorScheme = new MaterialData(Renderer);
        CurrentColorScheme.ChangeColor(INDICATOR_COLOR);
    }
    public virtual void Die()
    {
        if (IsDying)
            return;

        IsDying = true;

        OnDeathEvent.Invoke();

        Destroy(DestroyTarget);
    }
    protected virtual void OnValidate()
    {
        if (_destroyTarget == null)
            _destroyTarget = transform.parent == null ? gameObject : transform.parent.gameObject;

        if (_postProcessor)
            _postProcessor = GetComponent<HealthPostProcessor>();
    }
    protected virtual float GetStartHealth() => _postProcessor == null ? _startHealth.Value : _postProcessor.ProcessMaxHealth(_startHealth.Value);

    protected class MaterialData
    {
        public MaterialData(Renderer renderer)
        {
            _originalColors = new Dictionary<Material, Color>();

            foreach (Material material in renderer.materials)
            {
                _originalColors.Add(material, material.color);
            }
        }

        private readonly Dictionary<Material, Color> _originalColors;

        public void ChangeColor(Color color)
        {
            foreach (Material material in _originalColors.Keys)
            {
                material.color = color;
            }
        }
        public void Revert()
        {
            foreach (KeyValuePair<Material, Color> pair in _originalColors)
            {
                pair.Key.color = pair.Value;
            }
        }
    }
}
