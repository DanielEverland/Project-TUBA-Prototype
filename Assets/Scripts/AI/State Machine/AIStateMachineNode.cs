using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AIStateMachineNode : AIStateMachineObject
{
    public List<AIStateMachineTransition> Transitions => _transitions;

    [SerializeField]
    private List<AIStateMachineTransition> _transitions = new List<AIStateMachineTransition>();

#if UNITY_EDITOR
    public virtual Vector2 Position { get; set; }
    
    public virtual GUIStyle TextStyle => Styles.DefaultText;

    protected bool IsSelected => Selection.activeObject == this;
    protected abstract string Title { get; }
    public abstract Vector2 Size { get; }

    private const float FONT_SIZE_COEFFICIENT = 0.4f;

    public virtual void Draw(Rect rect)
    {
        DrawTitle(rect);
    }
    protected virtual void DrawTitle(Rect rect)
    {
        TextStyle.fontSize = (int)(rect.height * FONT_SIZE_COEFFICIENT);
        EditorGUI.LabelField(rect, new GUIContent(Title), TextStyle);
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
#endif
}