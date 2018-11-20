using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineStateNode : AIStateMachineNode
{
#if UNITY_EDITOR
    public override Vector2 Size => new Vector2(6, 1);

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
