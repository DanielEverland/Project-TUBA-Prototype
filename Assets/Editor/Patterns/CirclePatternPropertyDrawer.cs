using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CirclePattern))]
public class CirclePatternPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, "I'm a label :)");

        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.LabelField(position, "Hey, TopLabel! I'm below you :)");
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2;
    }
}
