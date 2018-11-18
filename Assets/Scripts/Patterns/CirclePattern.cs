using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CirclePattern : PatternComponent
{
    [SerializeField]
    private CircleData _data;

    protected CircleData Data => _data;
    protected const float RADIANS = 2 * Mathf.PI;

    public override void CreateChildren(GameObject parent, ref PatternObject pattern)
    {
        for (int i = 0; i < Data.Count; i++)
        {
            float angle = (RADIANS / Data.Count) * i;
            Vector2 position = new Vector2()
            {
                x = Data.Offset.x + Data.Radius * Mathf.Cos(angle),
                y = Data.Offset.y + Data.Radius * Mathf.Sin(angle),
            };

            GameObject instance = GameObject.Instantiate(Prefab, parent.transform);

            instance.transform.position = position;

            pattern.AddChild(instance);
        }
    }
    [System.Serializable]
	protected class CircleData
    {
        [SerializeField]
        private FloatReference _radius = new FloatReference(5);
        [SerializeField]
        private Vector2Reference _offset = new Vector2Reference(Vector2.zero);
        [SerializeField]
        private IntReference _count = new IntReference(10);

        public Vector2 Offset => _offset.Value;
        public float Radius => _radius.Value;
        public int Count => _count.Value;
    }
}
