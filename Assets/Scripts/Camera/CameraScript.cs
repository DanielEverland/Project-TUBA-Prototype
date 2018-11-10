using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObjectReference _target;
    [SerializeField]
    private FloatReference _distance = new FloatReference(10);

    protected GameObject Target => _target.Value;
    protected float Distance => _distance.Value;

    protected virtual void LateUpdate()
    {
        Vector3 targetPosition = Target.transform.position;
        targetPosition.z = -Distance;

        transform.position = targetPosition;
    }
}
