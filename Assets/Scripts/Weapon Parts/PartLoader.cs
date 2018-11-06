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

        LogData();
    }

    public static IEnumerable<TriggerData> TriggerData => _triggerData;
    public static IEnumerable<SeekerData> SeekerData =>_seekerData;
    public static IEnumerable<EventData> EventData => _eventData;

    private static List<TriggerData> _triggerData;
    private static List<SeekerData> _seekerData;
    private static List<EventData> _eventData;

    private static void LogData()
    {
        PrintInfo();
        CheckWarnings();
    }
    private static void PrintInfo()
    {
        string infoString = string.Format(
            "---Loading Parts---\n{0} triggers, {1} seekers, {2} events",
            _triggerData.Count, _seekerData.Count, _eventData.Count);

        Debug.Log(infoString);
    }
    private static void CheckWarnings()
    {
        WarnIfListIsEmpty(_triggerData, "Trigger Parts");
        WarnIfListIsEmpty(_seekerData, "Seeker Parts");
        WarnIfListIsEmpty(_eventData, "Event Parts");
    }
    private static void WarnIfListIsEmpty(ICollection collection, string collectionName)
    {
        if(collection.Count < 1)
        {
            Debug.LogError("No " + collectionName + " are loaded!");
        }
    }
}
