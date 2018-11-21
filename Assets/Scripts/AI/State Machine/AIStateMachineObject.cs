using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineObject : ScriptableObject
{
    public GameObject GameObject => Owner.GameObject;
    public AIStateMachine Owner { get => _owner; set => _owner = value; }

    [SerializeField]
    private AIStateMachine _owner;
}
