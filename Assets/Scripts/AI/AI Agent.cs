using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    [SerializeField]
    private IntReference _thinksPerSecond;
    [SerializeField]
    private AIStateMachine _stateMachine;

    protected AIStateMachine StateMachine => _stateMachine;
    protected int ThinksPerSecond => _thinksPerSecond.Value;
    protected float ThinksInterval => 1 / (float)ThinksPerSecond;
    protected float TimeSinceLastThink { get; set; }

    private void Awake()
    {

    }
    private void Update()
    {
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
            _thinksPerSecond.Value = Resources.Load<IntVariable>("DefaultThinksPerSecond");
    }
}
