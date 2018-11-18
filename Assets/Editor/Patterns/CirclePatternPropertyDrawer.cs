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
        CirclePattern pattern = property.objectReferenceValue as CirclePattern;
        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TOP_PADDING;
        
        pattern.Offset = EditorGUI.Vector2Field(position, new GUIContent("Offset"), pattern.Offset);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;
        
        pattern.Radius = EditorGUI.FloatField(position, new GUIContent("Radius"), pattern.Radius);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += SPACING;

        pattern.Count = EditorGUI.IntField(position, new GUIContent("Count"), pattern.Count);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * ELEMENTS + (SPACING * (ELEMENTS - 1)) + TOP_PADDING;
    }
}
