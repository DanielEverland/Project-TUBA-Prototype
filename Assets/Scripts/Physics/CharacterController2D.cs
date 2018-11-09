using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private FloatReference _drag = new FloatReference(0.1f);
    [SerializeField, Tooltip("Determines how much force is required to count as being pushed")]
    private FloatReference _minPushForce = new FloatReference(0.1f);
    [SerializeField]
    private BoolReference _disableMovementWhenPushed = new BoolReference(true);
    [SerializeField, HideInInspector]
    private Rigidbody2D _rigidbody;

    protected Vector2 MoveDelta { get; private set; }
    protected Vector2 CachedForce { get; private set; }
    protected Rigidbody2D Rigidbody => _rigidbody;
    protected bool DisableMovementWhenPushed => _disableMovementWhenPushed.Value;
    protected float Drag => _drag.Value;
    protected float MinPushForce => _minPushForce.Value;
    protected bool IsBeingPushed => CachedForce.magnitude > MinPushForce;
    
    public virtual void AddForce(Vector2 force)
    {
        CachedForce += force;
    }
    public virtual void Move(Vector2 direction)
    {
        MoveDelta += direction;
    }
    protected virtual void FixedUpdate()
    {
        Vector2 targetPosition = Vector2.zero;
        targetPosition += (Vector2)transform.position;
        targetPosition += CachedForce;
        
        if (!DisableMovementWhenPushed || !IsBeingPushed)
        {
            targetPosition += MoveDelta;
            MoveDelta = Vector2.zero;
        }
        
        Rigidbody.MovePosition(targetPosition);

        CachedForce = Vector2.MoveTowards(CachedForce, Vector2.zero, Drag);
    }
    protected virtual void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.hideFlags |= HideFlags.HideInInspector;
        _rigidbody.gravityScale = 0;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
