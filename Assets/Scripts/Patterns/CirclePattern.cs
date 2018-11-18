using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CirclePattern : PatternComponent
{
    [SerializeField]
    private Vector2Reference _offset = new Vector2Reference(Vector2.zero);
    [SerializeField]
    private FloatReference _radius = new FloatReference(5);
    [SerializeField]
    private IntReference _count = new IntReference(10);

    public Vector2 Offset => _offset.Value;
    public float Radius => _radius.Value;
    public int Count => _count.Value;

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
