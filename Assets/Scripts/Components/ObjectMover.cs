using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    private Space _space = Space.Self;
    [SerializeField]
    private Vector3 _direction = Vector3.right;
    [SerializeField]
    private FloatReference _velocity;
    [SerializeField, Tooltip("Should object be moved in Update or FixedUpdate?")]
    private bool _physicsUpdated = false;

    private void Update()
    {
        if (_physicsUpdated)
            return;

        transform.position += GetDirection() * _velocity.Value * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!_physicsUpdated)
            return;

        transform.position += GetDirection() * _velocity.Value;
    }
    private Vector3 GetDirection() => _space == Space.Self ? transform.TransformDirection(_direction) : _direction;
}