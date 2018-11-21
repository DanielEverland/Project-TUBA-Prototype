using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenericPropertyDrawer : PropertyDrawer
{
    public static GenericPropertyDrawer Drawer
    {
        get
        {
            if (_drawer == null)
                _drawer = new GenericPropertyDrawer();

            return _drawer;
        }
    }
    private static GenericPropertyDrawer _drawer;

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
        
        SerializedProperty element = serializedObject.GetIterator();
        if (element.NextVisible(true))
        {
            do
            {
                if (element != null)
                    EditorGUI.PropertyField(position, element);

                //if (ElementCount - i > 1)
                //{
                    position.y += SingleLineHeight;
                    position.y += Spacing;
                //}

            } while (element.NextVisible(true));
        }

        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return SingleLineHeight * ElementCount(property) + (Spacing * (ElementCount(property) - 1)) + TopPadding;
    }
    protected virtual int ElementCount(SerializedProperty property)
    {
        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        SerializedProperty prop = serializedObject.GetIterator();

        int i = 0;
        while (prop.NextVisible(true))
        {
            i++;
        }

        return i;
    }
}
