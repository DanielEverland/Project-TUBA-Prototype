using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads part assets from Resources into memory
/// </summary>
public static class PartLoader {

	static PartLoader()
    {
        _triggerData = Resources.LoadAll<TriggerData>("").ToList();
        _seekerData = Resources.LoadAll<SeekerData>("").ToList();
        _eventData = Resources.LoadAll<EventData>("").ToList();
    }

    public static IEnumerable<TriggerData> TriggerData { get { return _triggerData; } }
    public static IEnumerable<SeekerData> SeekerData { get { return _seekerData; } }
    public static IEnumerable<EventData> EventData { get { return _eventData; } }

    private static List<TriggerData> _triggerData;
    private static List<SeekerData> _seekerData;
    private static List<EventData> _eventData;
}
