using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CirclePattern))]
public class CirclePatternPropertyDrawer : PropertyDrawer
{
    private const float TOP_PADDING = 2;
    private const float SPACING = 2;
    private const int ELEMENTS = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TOP_PADDING;

        SerializedProperty offsetProperty = serializedObject.FindProperty("_offset");
        if (offsetProperty != null)
            EditorGUI.PropertyField(position, offsetProperty);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        SerializedProperty radiusProperty = serializedObject.FindProperty("_radius");
        if (radiusProperty != null)
            EditorGUI.PropertyField(position, radiusProperty);

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
