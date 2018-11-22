using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AINode : AIStateMachineObject
{
    public List<AITransition> Transitions => _transitions;

    [SerializeField]
    private List<AITransition> _transitions = new List<AITransition>();

    /// <summary>
    /// Called x amount of times per second for current state
    /// </summary>
    public virtual void Think()
    {

    }
    /// <summary>
    /// Called every frame for current state
    /// </summary>
    public virtual void Update()
    {

    }

#if UNITY_EDITOR
    public virtual Vector2 Position { get => _position; set => _position = value; }
    [SerializeField]
    private Vector2 _position;
    
    public virtual GUIStyle TextStyle => Styles.DefaultText;
    public virtual string Title => name;
    public abstract Vector2 MinSize { get; }

    protected bool IsSelected => Selection.activeObject == this;

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