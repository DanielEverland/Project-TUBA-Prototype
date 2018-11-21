using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineObject : ScriptableObject
{
    public GameObject GameObject => Machine.GameObject;
    public AIAgent Agent => Machine.Agent;
    public AIStateMachine Machine { get => _machine; set => _machine = value; }

    [SerializeField, HideInInspector]
    private AIStateMachine _machine;
}
