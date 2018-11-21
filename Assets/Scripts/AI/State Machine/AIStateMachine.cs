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

    public AIStateMachineNode CurrentState { get; protected set; }
    public AIAgent Agent { get; protected set; }
    public GameObject GameObject => Agent.gameObject;

    public void Initialize(AIAgent agent)
    {
        CurrentState = StartNode;
        Agent = agent;
    }
    public void Think()
    {
        CurrentState.Think();
        PollNextState();
    }
    public void Update()
    {
        CurrentState.Update();
    }
    private void PollNextState()
    {
        foreach (AIStateMachineTransition transition in CurrentState.Transitions)
        {
            if (transition.ConditionsMet)
            {
                CurrentState = transition.TargetState;
                return;
            }
        }
    }
}
