﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PatternMoveToPoint))]
public class PatternMoveToPointDrawer : PropertyDrawer
{
    private const float TOP_PADDING = 2;
    private const float SPACING = 2;
    private const int ELEMENTS = 2;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TOP_PADDING;

        SerializedProperty animationCurve = serializedObject.FindProperty("_curve");
        if(animationCurve != null)
            EditorGUI.PropertyField(position, animationCurve);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        SerializedProperty animationTime = serializedObject.FindProperty("_animationTime");
        if(animationTime != null)
            EditorGUI.PropertyField(position, animationTime);

        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * ELEMENTS + (SPACING * (ELEMENTS - 1)) + TOP_PADDING;
    }
}
