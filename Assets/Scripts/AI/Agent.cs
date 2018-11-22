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

    protected int ThinksPerSecond => _thinksPerSecond.Value;
    protected float ThinksInterval => 1 / (float)ThinksPerSecond;
    protected float TimeSinceLastThink { get; set; }

    private void Awake()
    {
        // We create a copy of the state machine
        _stateMachine = Instantiate(StateMachine);
        _stateMachine.Initialize(this);
    }
    private void Update()
    {
        Debug.Log(StateMachine.Agent);

        StateMachine.Update();
        PollThink();
    }
    private void PollThink()
    {
        TimeSinceLastThink += Time.deltaTime;

        if(TimeSinceLastThink > ThinksInterval)
        {
            TimeSinceLastThink -= ThinksInterval;

            StateMachine.Think();
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
