using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class AIStateMachineNode : ScriptableObject
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; } = new Vector2(3, 1);

    public virtual GUIStyle TextStyle => Styles.DefaultText;

    private const float FONT_SIZE_COEFFICIENT = 0.4f;

    public virtual void Draw(Rect rect)
    {
        DrawTitle(rect);
    }
    protected virtual void DrawTitle(Rect rect)
    {
        TextStyle.fontSize = (int)(rect.height * FONT_SIZE_COEFFICIENT);
        EditorGUI.LabelField(rect, new GUIContent("Start Node"), TextStyle);
    }

    private static class Styles
    {
        public static GUIStyle DefaultText;

        static Styles()
        {
            DefaultText = new GUIStyle();
            DefaultText.alignment = TextAnchor.MiddleCenter;
            DefaultText.normal.textColor = Color.white;
        }
    }
}