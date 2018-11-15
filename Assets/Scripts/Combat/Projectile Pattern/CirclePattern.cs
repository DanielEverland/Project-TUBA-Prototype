using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CirclePattern.asset", menuName = "Patterns/Circle", order = 520)]
public class CirclePattern : PatternShape
{
    [SerializeField]
    private List<CircleData> _circles;

    protected List<CircleData> Circles => _circles;
    protected const float RADIANS = 2 * Mathf.PI;

    protected override void CreateChildren(GameObject parent, ref PatternObject pattern)
    {
        int spawnCount = 0;

        for (int i = 0; i < Circles.Count; i++)
        {
            CircleData circleData = Circles[i];

            for (int j = 0; j < circleData.Count; j++)
            {
                float angle = (RADIANS / circleData.Count) * j;
                Vector2 position = new Vector2()
                {
                    x = circleData.Offset.x + circleData.Radius * Mathf.Cos(angle),
                    y = circleData.Offset.y + circleData.Radius * Mathf.Sin(angle),
                };

                GameObject instance = Instantiate(Prefab, parent.transform);

                instance.transform.position = position;
                instance.name = $"{Prefab.name} ({spawnCount})";

                pattern.AddChild(instance);
                spawnCount++;
            }
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
