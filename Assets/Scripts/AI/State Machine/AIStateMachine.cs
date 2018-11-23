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
    
    public void Think(Agent agent)
    {
        if(agent.CurrentObject is AINode node)
        {
            node.Think(agent);
            PollNextState(agent);
        }
        else if(agent.CurrentObject is AITransition transition)
        {
            if(transition.Transition(agent))
            {
                agent.ChangeCurrentObject(transition.TargetState);
            }
        }
    }
    public void PerformAction(Agent agent)
    {
        if(agent.CurrentObject is AIState state)
        {
            state.PerformAction(agent);
        }
    }
    private void PollNextState(Agent agent)
    {
        if(agent.CurrentObject is AINode node)
        {
            foreach (AITransition transition in node.Transitions)
            {
                if (transition.ConditionsMet)
                {
                    agent.ChangeCurrentObject(transition);
                    return;
                }
            }
        }
    }
    
}
