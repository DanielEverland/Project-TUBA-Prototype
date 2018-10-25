using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all data for a given weapon
/// </summary>[
[Serializable]
public class Weapon : ScriptableObject {
    
    public TriggerData TriggerData { get { return _triggerData; } set { _triggerData = value; } }
    public EventData EventData { get { return _eventData; } set { _eventData = value; } }
    public SeekerData SeekerData { get { return _seekerData; } set { _seekerData = value; } }

    [SerializeField]
    private TriggerData _triggerData;
    [SerializeField]
    private EventData _eventData;
    [SerializeField]
    private SeekerData _seekerData;
    
    public void ChangePart(PartBase part)
    {
        if(part is TriggerData)
        {
            TriggerData = part as TriggerData;
        }
        else if(part is EventData)
        {
            EventData = part as EventData;
        }
        else if(part is SeekerData)
        {
            SeekerData = part as SeekerData;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    public void AssignRandomParts()
    {
        _triggerData = PartLoader.TriggerData.Random();
        _eventData = PartLoader.EventData.Random();
        _seekerData = PartLoader.SeekerData.Random();
    }
}
