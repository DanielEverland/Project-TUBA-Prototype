using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SubdividerElement))]
public class SubdividerOnDeath : MonoBehaviour {

    [SerializeField]
    private Health _healthComponent;
    [SerializeField]
    private SubdividerElement _subdividerElement;
    [SerializeField]
    private SubdividerElement _prefab;
    [SerializeField]
    private IntReference _childrenToSpawn;

    protected Health HealthComponent => _healthComponent;
    protected SubdividerElement Prefab => _prefab;
    protected int CurrentLevel => _subdividerElement.CurrentLevel;
    protected int MaxLevel => _subdividerElement.MaxLevel;
    protected float ChildrenToSpawn => _childrenToSpawn.Value;
    
    public void OnDamaged()
    {
        if(HealthComponent.CurrentHealth <= 0)
        {
            DoSubdivide();
        }
    }
    private void DoSubdivide()
    {
        SpawnChildren();

        HealthComponent.Die();
    }
    private void SpawnChildren()
    {
        if (CurrentLevel >= MaxLevel)
            return;

        for (int i = 0; i < ChildrenToSpawn; i++)
        {
            SpawnChild();
        }
    }
    private void SpawnChild()
    {
        SubdividerElement spawnedChild = Instantiate(Prefab);


    }
    protected virtual void OnValidate()
    {
        if(_healthComponent == null)
            _healthComponent = GetComponent<Health>();

        if(_subdividerElement == null)
            _subdividerElement = GetComponent<SubdividerElement>();
    }
}
