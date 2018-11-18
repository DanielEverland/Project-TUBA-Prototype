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
        LinePattern pattern = property.objectReferenceValue as LinePattern;
        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TOP_PADDING;

        pattern.StartOffset = EditorGUI.Vector2Field(position, new GUIContent("Start"), pattern.StartOffset);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        pattern.EndOffset = EditorGUI.Vector2Field(position, new GUIContent("End"), pattern.EndOffset);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        pattern.Count = EditorGUI.IntField(position, new GUIContent("Count"), pattern.Count);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * ELEMENTS + (SPACING * (ELEMENTS - 1)) + TOP_PADDING;
    }
}
