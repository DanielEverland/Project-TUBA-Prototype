using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern.asset", menuName = "Pattern", order = 300)]
public class Pattern : ScriptableObject
{
    [SerializeField]
    private List<PatternComponent> _components;
    [SerializeField]
    private GameObject _prefab;
    
    public List<PatternComponent> Components => _components;
    public GameObject Prefab => _prefab;

    public PatternObject Spawn()
    {
        GameObject parent = new GameObject();
        parent.name = GetType().Name;

        PatternObject pattern = new PatternObject();
        
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
