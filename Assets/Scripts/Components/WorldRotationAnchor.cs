using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotationAnchor : MonoBehaviour
{
    [SerializeField]
    private FloatReference _angle = new FloatReference(0);

    private void Update() => transform.eulerAngles = Vector3.forward * _angle.Value;
}
