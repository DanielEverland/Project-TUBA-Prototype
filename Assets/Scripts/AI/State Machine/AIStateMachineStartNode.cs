using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineStartNode : AIStateMachineNode
{
    public override void Draw(Rect rect)
    {
        if(Event.current.type == EventType.Repaint)
            Style.Background.Draw(rect, GUIContent.none, 0);

        base.Draw(rect);
    }

    private static class Style
    {
        public static GUIStyle Background;

        static Style()
        {
            Background = new GUIStyle();
            Background.normal.background = Resources.Load<Texture2D>("Textures/StateMachineStartBackground");
            Background.border = new RectOffset(6, 6, 6, 0);
        }
    }
}
