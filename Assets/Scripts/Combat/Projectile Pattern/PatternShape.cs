using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternShape : ScriptableObject
{
    [SerializeField]
    private GameObject _childPrefab;

    protected GameObject Prefab => _childPrefab;

    public PatternObject Spawn()
    {
        GameObject parent = new GameObject();
        parent.name = GetType().Name;

        PatternObject pattern = new PatternObject();

        CreateChildren(parent, ref pattern);

        return pattern;
    }
    protected abstract void CreateChildren(GameObject parent, ref PatternObject pattern);

	public class PatternObject
    {
        public List<GameObject> Children { get; set; } = new List<GameObject>();

        public void AddChild(GameObject child)
        {
            Children.Add(child);
        }
    }
}
