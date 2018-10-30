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

    public int CurrentAmmo { get; set; }

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

            CurrentAmmo = TriggerData.AmmoCapacity;
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
        ChangePart(PartLoader.TriggerData.Random());
        ChangePart(PartLoader.EventData.Random());
        ChangePart(PartLoader.SeekerData.Random());
    }
}
