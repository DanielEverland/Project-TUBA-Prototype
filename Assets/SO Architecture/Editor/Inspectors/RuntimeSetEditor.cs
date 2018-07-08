﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(RuntimeSet<>), true)]
public class RuntimeSetEditor : Editor
{
    private ReorderableList _reorderableList;

    private void OnEnable()
    {
        SerializedProperty items = serializedObject.FindProperty("_items");

        _reorderableList = new ReorderableList(items.serializedObject, items);
        _reorderableList.drawElementCallback += DrawElement;
    }
    public override void OnInspectorGUI()
    {
        _reorderableList.DoLayoutList();

        _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
    }
    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        rect.height = EditorGUIUtility.singleLineHeight;
        rect.y++;

        SerializedProperty property = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.PropertyField(rect, property);

        property.serializedObject.ApplyModifiedProperties();
    }
}