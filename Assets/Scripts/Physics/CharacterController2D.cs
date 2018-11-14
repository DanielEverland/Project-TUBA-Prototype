using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private FloatReference _drag = new FloatReference(1);
    [SerializeField]
    private FloatReference _mass = new FloatReference(1);
    [SerializeField, HideInInspector]
    private Rigidbody2D _rigidbody;

    protected Vector2 MoveDelta { get; private set; }
    protected Vector2 Velocity { get => _rigidbody.velocity; private set => _rigidbody.velocity = value; }
    protected Rigidbody2D Rigidbody => _rigidbody;
    protected float Mass => _mass.Value;
    protected float Drag => _drag.Value;

    // No fucking idea why this is required.
    private const float MAGIC_FORCE_DIVIDER = 50;
    
    public virtual void AddForce(Vector2 force, ForceMode2D forceMode)
    {
        switch (forceMode)
        {
            case ForceMode2D.Force:
                Velocity += force / Mass / MAGIC_FORCE_DIVIDER * Time.fixedDeltaTime;
                break;
            case ForceMode2D.Impulse:
                Velocity += force / Mass / MAGIC_FORCE_DIVIDER;
                break;
            default:
                throw new System.NotImplementedException();
        }        
    }
    public virtual void Move(Vector2 direction)
    {
        MoveDelta += direction;
    }
    protected virtual void FixedUpdate()
    {
        Vector2 targetPosition = transform.position;
        targetPosition += Velocity;
        targetPosition += MoveDelta;
        
        Rigidbody.MovePosition(targetPosition);

        Velocity *= Mathf.Clamp01(1f - Drag * Time.fixedDeltaTime);
        MoveDelta = Vector2.zero;
    }
    protected virtual void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.hideFlags |= HideFlags.HideInInspector;
        _rigidbody.gravityScale = 0;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    protected virtual void OnCollisionStay2D(Collision2D collider)
    {
        if(transform.position == collider.transform.position)
        {
            Nudge();
        }
    }
    /// <summary>
    /// Nudge transform a tiny bit
    /// Used to avoid having objects directly on top of each other
    /// </summary>
    protected virtual void Nudge()
    {
        transform.position += new Vector3()
        {
            x = Random.Range(-0.01f, 0.01f),
            y = Random.Range(-0.01f, 0.01f),
        };
    }
}