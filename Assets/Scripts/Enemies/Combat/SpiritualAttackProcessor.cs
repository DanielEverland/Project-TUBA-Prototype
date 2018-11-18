using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpiritualModifier))]
public class SpiritualAttackProcessor : AIAttacker {

    [SerializeField, HideInInspector]
    private SpiritualModifier _spiritualModifier;
    [SerializeField]
    private AIAttacker _corperalAttacker;
    [SerializeField]
    private AIAttacker _spiritualAttacker;

    protected SpiritualModifier Modifier => _spiritualModifier;
    protected AIAttacker CorperalAttacker => _corperalAttacker;
    protected AIAttacker SpiritualAttacker => _spiritualAttacker;
    protected SpiritualState State => Modifier.CurrentState;
    
    public void PollAttackers()
    {
        SpiritualAttacker.CanAttack = State == SpiritualState.Spiritual;
        CorperalAttacker.CanAttack = State == SpiritualState.Corpereal;

        switch (State)
        {
            case SpiritualState.Corpereal:
                CorperalAttacker.Initialize();
                break;
            case SpiritualState.Spiritual:
                SpiritualAttacker.Initialize();
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
    protected virtual void OnValidate()
    {
        _spiritualModifier = GetComponent<SpiritualModifier>();
    }
}