using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectAdder : MonoBehaviour {

    [SerializeField]
    private GameObjectCollection _targetCollection;

    protected GameObjectCollection Collection => _targetCollection;

	protected virtual void Awake()
    {
        Collection.Add(gameObject);
    }
    protected virtual void OnDestroy()
    {
        Collection.Remove(gameObject);
    }
}
