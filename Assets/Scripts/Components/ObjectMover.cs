using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    private Space Space = UnityEngine.Space.Self;
    [SerializeField]
    private Vector3 Direction = Vector3.right;
    [SerializeField, Range(0, 1000)]
    private float Velocity = 10;
    [SerializeField, Tooltip("Should object be moved in Update or FixedUpdate?")]
    private bool PhysicsUpdated = false;

    private void Update()
    {
        if (PhysicsUpdated)
            return;

        transform.position += GetDirection() * Velocity * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!PhysicsUpdated)
            return;

        transform.position += GetDirection() * Velocity;
    }
    private Vector3 GetDirection() => Space == Space.Self ? transform.TransformDirection(Direction) : Direction;
}