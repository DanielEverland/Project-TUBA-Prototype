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

    public AIStartNode StartNode { get => _startNode; set => _startNode = value; }
    public List<AINode> Nodes => _nodes;
    public List<AITransition> Transitions => _transitions;

    [SerializeField]
    private AIStartNode _startNode;
    [SerializeField]
    private List<AINode> _nodes = new List<AINode>();
    [SerializeField]
    private List<AITransition> _transitions = new List<AITransition>();

    protected AIState CurrentState => CurrentObject as AIState;
    protected AITransition CurrentTransition => CurrentObject as AITransition;
    
    public AIStateMachineObject CurrentObject { get; protected set; }
    public Agent Agent { get; protected set; }
    public GameObject GameObject => Agent.gameObject;
    protected bool IsInitialized { get; set; }

    public void Initialize(Agent agent)
    {
        IsInitialized = true;
        CurrentObject = StartNode;
        Agent = agent;

        foreach (AIStateMachineObject machineObject in Nodes.Select(x => x as AIStateMachineObject).Union(Transitions))
        {
            machineObject.Initialize(this);
        }
    }
    public void Think()
    {
        if (!IsInitialized)
            return;

        if(CurrentObject is AINode node)
        {
            node.Think();
            PollNextState();
        }
        else if(CurrentObject is AITransition transition)
        {
            if(transition.Transition())
            {
                ChangeCurrentObject(transition.TargetState);
            }
        }
    }
    public void Update()
    {
        if (!IsInitialized)
            return;

        CurrentState?.Update();
    }
    private void PollNextState()
    {
        if(CurrentObject is AINode node)
        {
            foreach (AITransition transition in node.Transitions)
            {
                if (transition.ConditionsMet)
                {
                    ChangeCurrentObject(transition);
                    return;
                }
            }
        }
    }
    public void ChangeCurrentObject(AIStateMachineObject obj)
    {
        if (CurrentState != null)
        {
            CurrentState.StateEnded();
        }
        else if (CurrentTransition != null)
        {
            CurrentTransition.TransitionEnded();
        }

        CurrentObject = obj;

        if (CurrentState != null)
        {
            CurrentState.StateStarted();
        }
        else if (CurrentTransition != null)
        {
            CurrentTransition.TransitionStarted();
        }
    }
}
