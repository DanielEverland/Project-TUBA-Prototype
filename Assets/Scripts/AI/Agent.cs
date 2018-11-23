using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private IntReference _thinksPerSecond;
    [SerializeField]
    private AIStateMachine _stateMachine;
    [SerializeField]
    private CharacterController2D _characterController;
    
    public CharacterController2D CharacterController => _characterController;
    public AIStateMachine StateMachine => _stateMachine;
    public AIStateMachineObject CurrentObject { get; protected set; }

    protected int ThinksPerSecond => _thinksPerSecond.Value;
    protected float ThinksInterval => 1 / (float)ThinksPerSecond;
    protected float TimeSinceLastThink { get; set; }
    
    private void Update()
    {
        StateMachine.PerformAction(this);
        PollThink();
    }
    private void PollThink()
    {
        TimeSinceLastThink += Time.deltaTime;

        if(TimeSinceLastThink > ThinksInterval)
        {
            TimeSinceLastThink -= ThinksInterval;

            StateMachine.Think(this);
        }
    }
    public void ChangeCurrentObject(AIStateMachineObject newObject)
    {
        if (newObject == CurrentObject)
            return;

        HandleObjectEnded();
        
        CurrentObject = newObject;

        HandleObjectStarted();
    }
    private void HandleObjectStarted()
    {
        if(CurrentObject is AIState state)
        {
            state.StateStarted(this);
        }
        else if(CurrentObject is AITransition transition)
        {
            transition.TransitionStarted(this);
        }
    }
    private void HandleObjectEnded()
    {
        if (CurrentObject is AIState state)
        {
            state.StateEnded(this);
        }
        else if (CurrentObject is AITransition transition)
        {
            transition.TransitionEnded(this);
        }
    }
    private void OnValidate()
    {
        if (_thinksPerSecond == null)
            _thinksPerSecond = new IntReference(Resources.Load<IntVariable>("DefaultThinksPerSecond").Value);

        if (_characterController == null)
            _characterController = GetComponent<CharacterController2D>();
    }
}
