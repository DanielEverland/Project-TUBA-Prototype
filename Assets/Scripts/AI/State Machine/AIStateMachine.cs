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

    public AIStateMachineStartNode StartNode { get => _startNode; set => _startNode = value; }
    public List<AIStateMachineNode> Nodes => _nodes;
    public List<AIStateMachineTransition> Transitions => _transitions;

    [SerializeField]
    private AIStateMachineStartNode _startNode;
    [SerializeField]
    private List<AIStateMachineNode> _nodes = new List<AIStateMachineNode>();
    [SerializeField]
    private List<AIStateMachineTransition> _transitions = new List<AIStateMachineTransition>();

    public AIStateMachineNode CurrentNode { get; protected set; }
    public GameObject GameObject { get; protected set; }

    public void Initialize(GameObject owner)
    {
        CurrentNode = StartNode;
        GameObject = owner;
    }
    public void Think()
    {
        foreach (AIStateMachineTransition transition in CurrentNode.Transitions)
        {
            throw new System.NotImplementedException();
        }
    }
}
