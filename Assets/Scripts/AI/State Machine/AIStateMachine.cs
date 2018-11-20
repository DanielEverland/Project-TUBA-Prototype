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
    public List<AIStateMachineTransition> Transitions => _transitions;

    [SerializeField, HideInInspector]
    private List<AIStateMachineNode> _nodes = new List<AIStateMachineNode>();
    [SerializeField, HideInInspector]
    private List<AIStateMachineTransition> _transitions = new List<AIStateMachineTransition>();
}
