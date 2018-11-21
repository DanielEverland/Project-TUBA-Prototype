﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AIStateMachineTransition : AIStateMachineObject
{
    public AIStateMachineStateNode TargetState;

#if UNITY_EDITOR
    public AIStateMachineNode StartNode;

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

    public void Draw(Rect rect)
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
        }
    }
    private static class Styles
    {
        public static GUIStyle TransitionBackground;
        public static GUIStyle TransitionArrow;

        public static GUIStyle SelectedTransitionBackground;
        public static GUIStyle SelectedTransitionArrow;

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
        }
    }
#endif
}