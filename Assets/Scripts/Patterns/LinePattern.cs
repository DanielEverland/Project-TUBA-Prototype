using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePattern : PatternComponent
{
    [SerializeField]
    private Vector2Reference _startOffset = new Vector2Reference(new Vector2(-5, 0));
    [SerializeField]
    private Vector2Reference _endOffset = new Vector2Reference(new Vector2(5, 0));
    [SerializeField]
    private IntReference _count = new IntReference(6);

    public Vector2 StartOffset => _startOffset.Value;
    public Vector2 EndOffset => _endOffset.Value;
    public int Count => _count.Value;

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
