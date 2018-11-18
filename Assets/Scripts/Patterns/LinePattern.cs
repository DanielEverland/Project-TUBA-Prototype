using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePattern : PatternComponent
{
    public Vector2 StartOffset { get; set; } = new Vector2(-5, 0);
    public Vector2 EndOffset { get; set; } = new Vector2(5, 0);
    public int Count { get; set; } = 6;

    public override void CreateChildren(GameObject parent, GameObject prefab, ref PatternObject pattern)
    {
        float totalDistance = Vector2.Distance(StartOffset, EndOffset);
        float distanceBetweenObjects = totalDistance / Count;

        for (int i = 0; i < Count; i++)
        {
            float percentage = (distanceBetweenObjects * i) / totalDistance;
            Vector2 position = Vector2.Lerp(StartOffset, EndOffset, percentage);

            GameObject instance = GameObject.Instantiate(prefab, parent.transform);

            instance.transform.position = position;

            pattern.AddChild(instance);
        }
    }
}
