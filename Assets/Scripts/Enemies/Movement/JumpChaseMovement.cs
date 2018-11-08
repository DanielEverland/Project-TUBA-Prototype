using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChaseMovement : ChaseMovement
{
    [SerializeField]
    private FloatReference _jumpTime;
    [SerializeField]
    private FloatReference _pauseBetweenJump;    
    [SerializeField]
    private AnimationCurve _jumpPositionAnimation = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected Color CurrentJumpPositionColor => Color.cyan;
    protected Color JumpDeltaColor => Color.blue;

    protected float JumpTime => _jumpTime.Value;
    protected float TimePassedSinceLastJump => Time.time - _lastJumpTime;
    protected float PauseBetweenJumps => _pauseBetweenJump.Value;
    
    private float _lastJumpTime = float.MinValue;
    private Vector3 _currentJumpTarget;
    private Vector3 _currentJumpStartPosition;

    protected override void Update()
    {
        if (IsWithinRange)
            return;

        if(TimePassedSinceLastJump >= JumpTime + PauseBetweenJumps)
        {
            SelectNewJumpPosition();
        }
        else
        {
            Move();
        }

        DrawDebug();
    }
    protected override void DrawDebug()
    {
        base.DrawDebug();

        Debug.DrawLine(_currentJumpStartPosition, _currentJumpTarget, JumpDeltaColor);
        Debug.DrawLine(transform.position, _currentJumpTarget, CurrentJumpPositionColor);
    }
    private void SelectNewJumpPosition()
    {
        _lastJumpTime = Time.time;
        _currentJumpTarget = transform.position + Direction * MovementSpeed;
        _currentJumpStartPosition = transform.position;
    }
    private void Move()
    {
        float animationTime = TimePassedSinceLastJump / JumpTime;
        float animationValue = Mathf.Clamp(_jumpPositionAnimation.Evaluate(animationTime), 0, 1);
        
        Vector3 framePositionTarget = Vector3.Lerp(_currentJumpStartPosition, _currentJumpTarget, animationValue);
        Vector3 delta = framePositionTarget - transform.position;

        Move(delta.normalized, delta.magnitude);
    }
}
