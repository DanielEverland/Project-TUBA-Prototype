using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIStateMachineStateNode : AIStateMachineNode
{
    /// <summary>
    /// States can override this if a transition should check whether it has completed its action
    /// </summary>
    public bool IsDone => true;
    public UnityEvent OnStateStarted => _onStateStarted;
    public UnityEvent OnStateEnded => _onStateEnded;

    public List<AIStateMachineAction> Actions => _actions;
    [SerializeField]
    private List<AIStateMachineAction> _actions = new List<AIStateMachineAction>();
    [SerializeField]
    private UnityEvent _onStateStarted;
    [SerializeField]
    private UnityEvent _onStateEnded;

    public override void Initialize(AIStateMachine machine)
    {
        foreach (AIStateMachineObject action in Actions)
        {
            action.Initialize(machine);
        }
    }
    public virtual void StateStarted()
    {
        OnStateStarted.Invoke();
    }
    public virtual void StateEnded()
    {
        OnStateEnded.Invoke();
    }
    public override void Update()
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].Update();
        }
    }
    public override void Think()
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].Think();
        }
    }

#if UNITY_EDITOR
    public override Vector2 MinSize => new Vector2(2, 1);
    public bool TransitionsFoldout = false;

    public override void Draw(Rect rect)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (IsSelected)
                Style.SelectedBackground.Draw(rect, GUIContent.none, 0);
            else if (IsCurrent)
                Style.CurrentBackground.Draw(rect, GUIContent.none, 0);
            else
                Style.Background.Draw(rect, GUIContent.none, 0);
        }

        base.Draw(rect);
    }

    private static class Style
    {
        public static GUIStyle Background;
        public static GUIStyle SelectedBackground;
        public static GUIStyle CurrentBackground;

        static Style()
        {
            Background = new GUIStyle();
            Background.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStateBackground");
            Background.border = new RectOffset(4, 4, 4, 4);

            SelectedBackground = new GUIStyle();
            SelectedBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStateBackgroundOutline");
            SelectedBackground.border = new RectOffset(4, 4, 4, 4);

            CurrentBackground = new GUIStyle();
            CurrentBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStateBackgroundCurrent");
            CurrentBackground.border = new RectOffset(4, 4, 4, 4);
        }
    }
#endif
}
