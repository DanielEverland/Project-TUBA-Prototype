using UnityEngine;

public class AIStartNode : AINode
{
#if UNITY_EDITOR
    public override Vector2 MinSize => new Vector2(3, 1);

    public override void Draw(Rect rect, Agent agent)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if(agent?.CurrentObject == this)
                Style.CurrentBackground.Draw(rect, GUIContent.none, 0);
            else if (IsSelected)
                Style.SelectedBackground.Draw(rect, GUIContent.none, 0);
            else
                Style.Background.Draw(rect, GUIContent.none, 0);
        }

        base.DrawTitle(rect);
    }
    
    private static class Style
    {
        public static GUIStyle Background;
        public static GUIStyle SelectedBackground;
        public static GUIStyle CurrentBackground;

        static Style()
        {
            Background = new GUIStyle();
            Background.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStartBackground");
            Background.border = new RectOffset(6, 6, 6, 0);

            SelectedBackground = new GUIStyle();
            SelectedBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStartBackgroundOutline");
            SelectedBackground.border = new RectOffset(6, 6, 6, 0);

            CurrentBackground = new GUIStyle();
            CurrentBackground.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStartBackgroundCurrent");
            CurrentBackground.border = new RectOffset(6, 6, 6, 0);
        }
    }
#endif
}