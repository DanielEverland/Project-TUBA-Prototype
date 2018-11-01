using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDebugging : MonoBehaviour
{
    [SerializeField]
    private Vector3Reference _direction;

    private const float DEBUG_RAY_LENGTH = 3;

    private void LateUpdate()
    {
        DrawDebug();
    }
    private void DrawDebug()
    {
        Debug.DrawRay(transform.position, _direction.Value * DEBUG_RAY_LENGTH, Color.cyan);
    }
}