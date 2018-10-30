using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData : EventDataBase, IGameEventListener
{
    [SerializeField]
    private GameEvent _gameEvent;

    public virtual void OnEventRaised() { }
    public override void OnEquipped()
    {
        _gameEvent.AddListener(this);
    }
    public override void OnUneqipped()
    {
        _gameEvent.RemoveListener(this);
    }
}