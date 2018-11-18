using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CirclePattern : PatternComponent
{
    public Vector2 Offset { get; set; }
    public float Radius { get; set; } = 5;
    public int Count { get; set; } = 10;

    protected const float RADIANS = 2 * Mathf.PI;

    public override void CreateChildren(GameObject parent, GameObject prefab, ref PatternObject pattern)
    {
        for (int i = 0; i < Count; i++)
        {
            float angle = (RADIANS / Count) * i;
            Vector2 position = new Vector2()
            {
                x = Offset.x + Radius * Mathf.Cos(angle),
                y = Offset.y + Radius * Mathf.Sin(angle),
            };

            GameObject instance = GameObject.Instantiate(prefab, parent.transform);

            instance.transform.position = position;

            pattern.AddChild(instance);
        }
    }
}
