using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class ReorderableListElementPropertyDrawer : PropertyDrawer
{
    protected abstract List<string> ElementNames { get; }

    protected virtual int ElementCount => ElementNames.Count;
    protected virtual float SingleLineHeight => EditorGUIUtility.singleLineHeight;
    protected virtual float TopPadding => 2;
    protected virtual float Spacing => 2;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = SingleLineHeight;
        position.y += TopPadding;

        for (int i = 0; i < ElementCount; i++)
        {
            SerializedProperty element = serializedObject.FindProperty(ElementNames[i]);
            if (element != null)
                EditorGUI.PropertyField(position, element);

            if (ElementCount - i > 1)
            {
                position.y += SingleLineHeight;
                position.y += Spacing;
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return SingleLineHeight * ElementCount + (Spacing * (ElementCount - 1)) + TopPadding;
    }
}
