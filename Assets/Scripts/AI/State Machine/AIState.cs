using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIState : AINode
{
    public List<AIAction> Actions => _actions;
    [SerializeField]
    private List<AIAction> _actions = new List<AIAction>();
    
    public virtual void StateStarted(Agent agent)
    {
    }
    public virtual void StateEnded(Agent agent)
    {
    }
    public override void PerformAction(Agent agent)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].PerformAction(agent);
        }
    }
    public override void Think(Agent agent)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].Think(agent);
        }
    }

#if UNITY_EDITOR
    public override Vector2 MinSize => new Vector2(2, 1);
    public bool TransitionsFoldout = false;

    public override void Draw(Rect rect, Agent agent)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if(agent?.CurrentObject == this)
                Style.CurrentBackground.Draw(rect, GUIContent.none, 0);
            else if(IsSelected)
                Style.SelectedBackground.Draw(rect, GUIContent.none, 0);
            else
                Style.Background.Draw(rect, GUIContent.none, 0);                
        }

        base.Draw(rect, agent);
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
