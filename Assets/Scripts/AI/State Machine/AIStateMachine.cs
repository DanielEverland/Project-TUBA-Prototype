using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIStateMachine.asset", menuName = "AI State Machine", order = 301)]
public class AIStateMachine : ScriptableObject
{
#if UNITY_EDITOR
    public Vector2 CameraOffset;
    public float CameraScale;
#endif

    public List<AIStateMachineNode> Nodes => _nodes;
    [SerializeField, HideInInspector]
    private List<AIStateMachineNode> _nodes = new List<AIStateMachineNode>();
}
