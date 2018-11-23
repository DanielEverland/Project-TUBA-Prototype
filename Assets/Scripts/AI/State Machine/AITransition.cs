using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AITransition : AIStateMachineObject
{
    public AIState TargetState { get => _targetState; set => _targetState = value; }
    public List<AICondition> Conditions => _conditions;
    public ConditionType ExpressionType => _expressionType;
    public float TransitionTime => _transitionTime.Value;

    [SerializeField]
    private ConditionType _expressionType = ConditionType.All;
    [SerializeField]
    private AIState _targetState;
    [SerializeField]
    private List<AICondition> _conditions = new List<AICondition>();
    [SerializeField]
    private FloatReference _transitionTime = new FloatReference(1);
    
    public bool ConditionsMet
    {
        get
        {
            switch (ExpressionType)
            {
                case ConditionType.All:
                    {
                        for (int i = 0; i < Conditions.Count; i++)
                            if (Conditions[i] == false)
                                return false;

                        return true;
                    }
                case ConditionType.Any:
                    {
                        for (int i = 0; i < Conditions.Count; i++)
                            if (Conditions[i] == true)
                                return true;

                        return false;
                    }
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
    
    public void TransitionStarted(Agent agent)
    {
        SetData(agent, AIDataTypes.TransitionStartTime, Time.time);
    }
    public void TransitionEnded(Agent agent)
    {
    }
    public bool Transition(Agent agent)
    {
        float startTime = GetData<float>(agent, AIDataTypes.TransitionStartTime);

        return Time.time - startTime >= TransitionTime;
    }

    [System.Serializable]
    public enum ConditionType
    {
        All,
        Any,
    }
    
#if UNITY_EDITOR
    public AINode StartNode;

    public Vector2 StartPosition => StartNode.Position;
    public Vector2 EndPosition
    {
        get
        {
            if (TargetState == null)
                return _endPosition;

            return TargetState.Position;
        }
        set
        {
            _endPosition = value;
        }
    }
    private Vector2 _endPosition;

    public void Draw(Rect rect, Agent agent)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Vector2 arrowSize = new Vector2(Styles.TransitionArrow.normal.background.width, Styles.TransitionArrow.normal.background.height);
            Rect arrowRect = new Rect()
            {
                position = rect.center - arrowSize / 2,
                size = arrowSize,
            };

            if(Selection.activeObject == this)
            {
                Styles.SelectedTransitionArrow.Draw(arrowRect, GUIContent.none, 0);
                Styles.SelectedTransitionBackground.Draw(rect, GUIContent.none, 0);
            }
            else
            {
                Styles.TransitionArrow.Draw(arrowRect, GUIContent.none, 0);
                Styles.TransitionBackground.Draw(rect, GUIContent.none, 0);
            }

            if(agent?.CurrentObject == this)
                DrawCurrentBackground(rect, agent);
        }
    }
    private void DrawCurrentBackground(Rect rect, Agent agent)
    {
        float transitionStartTime = GetData<float>(agent, AIDataTypes.TransitionStartTime);
        float timePassed = Time.time - transitionStartTime;
        float percentage = Mathf.Clamp01(timePassed / TransitionTime);

        rect.width *= percentage;

        Styles.CurrentBackground.Draw(rect, GUIContent.none, 0);
    }
    private static class Styles
    {
        public static GUIStyle TransitionBackground;
        public static GUIStyle TransitionArrow;

        public static GUIStyle SelectedTransitionBackground;
        public static GUIStyle SelectedTransitionArrow;

        public static GUIStyle CurrentBackground;

        static Styles()
        {
            TransitionBackground = new GUIStyle();
            TransitionBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineTransitionBackground");

            TransitionArrow = new GUIStyle();
            TransitionArrow.normal.background = Resources.Load<Texture2D>("Textures/StateMachineTransitionArrow");


            SelectedTransitionBackground = new GUIStyle();
            SelectedTransitionBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineTransitionBackgroundSelected");

            SelectedTransitionArrow = new GUIStyle();
            SelectedTransitionArrow.normal.background = Resources.Load<Texture2D>("Textures/StateMachineTransitionArrowSelected");


            CurrentBackground = new GUIStyle();
            CurrentBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineTransitionBackgroundCurrent");
        }
    }
#endif
}
