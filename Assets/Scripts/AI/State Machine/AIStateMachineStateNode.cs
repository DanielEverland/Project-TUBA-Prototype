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
            else
                Style.Background.Draw(rect, GUIContent.none, 0);
        }

        base.Draw(rect);
    }

    private static class Style
    {
        public static GUIStyle Background;
        public static GUIStyle SelectedBackground;

        static Style()
        {
            Background = new GUIStyle();
            Background.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStateBackground");
            Background.border = new RectOffset(6, 6, 6, 0);

            SelectedBackground = new GUIStyle();
            SelectedBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStateBackgroundOutline");
            SelectedBackground.border = new RectOffset(6, 6, 6, 0);
        }
    }
#endif
}
