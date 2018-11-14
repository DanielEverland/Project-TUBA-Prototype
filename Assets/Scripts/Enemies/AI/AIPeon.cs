using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPeon : MonoBehaviour {

    [SerializeField]
    private FloatReference _thinksPerSecond = new FloatReference(10);
    [SerializeField, HideInInspector]
    private List<IAIComponent> _aiComponents;

    protected List<IAIComponent> AIComponents => _aiComponents;
    protected float ThinksPerSecond => _thinksPerSecond.Value;
    protected float ThinkInterval => 1 / ThinksPerSecond;
    protected float TimePassed { get; set; }

    protected virtual void Update()
    {
        ProcessTime();
    }
    protected virtual void ProcessTime()
    {
        TimePassed += Time.deltaTime;

        if(TimePassed > ThinkInterval)
        {
            TimePassed -= ThinkInterval;

            Think();
        }
    }
    protected virtual void Think()
    {
        for (int i = AIComponents.Count - 1; i >= 0; i--)
        {
            AIComponents[i].Think();
        }
    }
    public void AddComponent(IAIComponent component)
    {
        if(!AIComponents.Contains(component))
            AIComponents.Add(component);
    }
    public void RemoveComponent(IAIComponent component)
    {
        if (AIComponents.Contains(component))
            AIComponents.Remove(component);
    }
    protected virtual void OnValidate()
    {
        _aiComponents = new List<IAIComponent>(GetComponentsInChildren<IAIComponent>());
    }
}
