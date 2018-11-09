using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChaseMovement : ChaseMovement
{
    [SerializeField]
    private FloatReference _pauseBetweenJump;    
    [SerializeField]
    private AnimationCurve _jumpPositionAnimation = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected Color CurrentJumpPositionColor => Color.cyan;
    protected Color JumpDeltaColor => Color.blue;
    
    protected float TimePassedSinceLastJump => Time.time - _lastJumpTime;
    protected float PauseBetweenJumps => _pauseBetweenJump.Value;
    
    private float _lastJumpTime = float.MinValue;

    protected override void Update()
    {
        if (IsWithinRange)
            return;

        if(TimePassedSinceLastJump >= PauseBetweenJumps)
        {
            Jump();
        }

        DrawDebug();
    }
    private void Jump()
    {
        _lastJumpTime = Time.time;
        
        CharacterController.AddForce(Direction * MovementSpeed);
    }
}
