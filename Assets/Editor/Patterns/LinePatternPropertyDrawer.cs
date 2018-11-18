using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LinePattern))]
public class LinePatternPropertyDrawer : PropertyDrawer
{
    private const float TOP_PADDING = 2;
    private const float SPACING = 2;
    private const int ELEMENTS = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TOP_PADDING;

        SerializedProperty startOffsetProperty = serializedObject.FindProperty("_startOffset");
        if(startOffsetProperty != null)
            EditorGUI.PropertyField(position, startOffsetProperty);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        SerializedProperty endOffsetProperty = serializedObject.FindProperty("_endOffset");
        if (endOffsetProperty != null)
            EditorGUI.PropertyField(position, endOffsetProperty);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        SerializedProperty countProperty = serializedObject.FindProperty("_count");
        if (countProperty != null)
            EditorGUI.PropertyField(position, countProperty);

        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * ELEMENTS + (SPACING * (ELEMENTS - 1)) + TOP_PADDING;
    }
}
