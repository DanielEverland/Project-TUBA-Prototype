using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern.asset", menuName = "Pattern", order = 300)]
public class Pattern : ScriptableObject
{
    [SerializeField]
    private List<PatternComponent> _components;
    [SerializeField]
    private PatternBehaviour _behaviour;
    [SerializeField]
    private GameObject _prefab;

#if UNITY_EDITOR
    public bool BehaviourFoldoutState = true;
#endif

    public List<PatternComponent> Components => _components;
    public PatternBehaviour Behaviour { get => _behaviour; set => _behaviour = value; }
    public GameObject Prefab => _prefab;
    
    public PatternObject Spawn()
    {
        GameObject parent = new GameObject();
        parent.name = GetType().Name;

        PatternObject pattern = new PatternObject(parent);

        if(Behaviour != null)
        {
            PatternBehaviourExecutor executor = parent.AddComponent<PatternBehaviourExecutor>();
            executor.Initialize(Behaviour, pattern);
        }        
        
        foreach (PatternComponent component in Components)
        {
            component.CreateChildren(parent, Prefab, ref pattern);
        }

        for (int i = 0; i < pattern.Children.Count; i++)
        {
            pattern.Children[i].name = $"Pattern ({i})";
        }

        return pattern;
    }
}
