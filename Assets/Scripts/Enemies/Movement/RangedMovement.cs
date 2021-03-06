﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedMovement : AIMover
{
    [SerializeField]
    private FloatReference _actTime = new FloatReference(5);
    [SerializeField]
    private FloatReference _pauseTime = new FloatReference(1.5f);
    [SerializeField]
    private FloatReference _preferredDistance = new FloatReference(10);
    [SerializeField]
    private FloatReference _minDistance = new FloatReference(5);
    [SerializeField]
    private ActState _defaultActState = ActState.Move;
    [SerializeField]
    private AIAttacker _attacker;

    [Space()]

    [SerializeField]
    private ActStateEvent _onStateChanged;

    private static readonly Color PreferredPositionColor = new Color(0, 0, 1, 0.3f);
    
    protected Vector2 TargetPosition { get; set; }
    protected ActState CurrentActState { get; set; }
    protected ActState DefaultActState => _defaultActState;
    protected AIAttacker Attacker => _attacker;
    protected Vector2 PreferredPosition => PlayerPosition - PlayerDirection * PreferredDistance;
    protected bool IsActing => TimeSinceLastDecision < ActTime;
    protected float TimeBetweenDecisions => ActTime + PauseTime;
    protected float TimeSinceLastDecision { get; set; }
    protected float ActTime => _actTime.Value;
    protected float PauseTime => _pauseTime.Value;
    protected float PreferredDistance => _preferredDistance.Value;
    protected float MinDistance => _minDistance.Value;

    protected virtual void Awake()
    {
        SetState(DefaultActState);
    }
    protected override void Update()
    {
        base.Update();

        if(IsActing)
            PerformAction();

        TimeSinceLastDecision += Time.deltaTime;
    }
    protected virtual void PerformAction()
    {
        switch (CurrentActState)
        {
            case ActState.Move:
                DoRun();
                break;
            case ActState.Attack:
                DoAttack();
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
    protected virtual void DoRun()
    {
        MoveTo(TargetPosition);
    }
    protected virtual void DoAttack()
    {
        Attacker.CanAttack = true;
    }
    public override void Think()
    {
        base.Think();

        if(TimeSinceLastDecision > TimeBetweenDecisions)
        {
            MakeDecision();
        }
    }
    protected virtual void MakeDecision()
    {
        SetState(CurrentActState.Next());

        TimeSinceLastDecision -= TimeBetweenDecisions;
    }
    protected virtual void SetState(ActState state)
    {
        CurrentActState = state;
        
        switch (state)
        {
            case ActState.Move:
                {
                    TargetPosition = PreferredPosition;
                }
                break;
        }

        Attacker.CanAttack = false;

        _onStateChanged.Invoke(CurrentActState);
    }
    protected override void DrawDebug()
    {
        base.DrawDebug();

        Debug.DrawLine(transform.position, PreferredPosition, PreferredPositionColor);
    }
    protected override void OnValidate()
    {
        base.OnValidate();

        if (_attacker == null)
            _attacker = GetComponent<AIAttacker>();
    }
    
    public enum ActState
    {
        Move = 0,
        Attack = 1,
    }

    [System.Serializable]
    public class ActStateEvent : UnityEvent<ActState>
    {

    }
}
