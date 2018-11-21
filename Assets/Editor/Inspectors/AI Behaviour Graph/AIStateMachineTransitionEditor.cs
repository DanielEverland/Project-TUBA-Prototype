﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIStateMachineTransition))]
public class AIStateMachineTransitionEditor : Editor
{
    protected AIStateMachineTransition Target { get { return (AIStateMachineTransition)target; } }
    protected SerializedProperty Conditions { get; set; }
    protected ReorderableList ConditionsList { get; set; }
    protected string[] TypeOptions { get; set; }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ConditionsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void DrawElement(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        int index = ConditionsList.IndexOf(element);
        AIStateMachineCondition condition = Target.Conditions[index];
        PropertyDrawer drawer = PropertyDrawerLoader.Drawers[condition.GetType()];
        System.Type type = condition.GetType();
        int typeIndex = ConditionsLoader.AllTypes.IndexOf(type);

        rect.height = EditorGUIUtility.singleLineHeight;
        
        int newIndex = EditorGUI.Popup(rect, typeIndex, TypeOptions);

        rect.y += EditorGUIUtility.singleLineHeight;

        drawer.OnGUI(rect, element, GUIContent.none);

        if(newIndex != typeIndex)
        {
            RemoveObject(condition);
            Target.Conditions.Remove(condition);

            AIStateMachineCondition newCondition = CreateNewCondition(ConditionsLoader.AllTypes[newIndex]);
            Target.Conditions.Insert(index, newCondition);

            AddObject(newCondition);
        }
    }
    protected virtual void AddCondition(ReorderableList list)
    {
        AIStateMachineCondition newCondition = CreateNewCondition(ConditionsLoader.AllTypes[0]);
        Target.Conditions.Add(newCondition);

        AddObject(newCondition);
    }
    protected virtual AIStateMachineCondition CreateNewCondition(System.Type type)
    {
        AIStateMachineCondition newCondition = (AIStateMachineCondition)ScriptableObject.CreateInstance(type);

        newCondition.name = newCondition.GetType().Name;

        return newCondition;
    }
    protected virtual void AddObject(Object obj)
    {
        AssetDatabase.AddObjectToAsset(obj, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
    }
    protected virtual void RemoveCondition(ReorderableList list)
    {
        AIStateMachineCondition toRemove = Conditions.GetArrayElementAtIndex(list.Index).objectReferenceValue as AIStateMachineCondition;

        Target.Conditions.Remove(toRemove);
        RemoveObject(toRemove);
    }
    protected virtual void RemoveObject(Object obj)
    {
        DestroyImmediate(obj, true);
    }
    protected virtual float GetElementHeight(SerializedProperty element)
    {
        int index = ConditionsList.IndexOf(element);
        return PropertyDrawerLoader.Drawers[Target.Conditions[index].GetType()].GetPropertyHeight(element, GUIContent.none) + EditorGUIUtility.singleLineHeight;
    }
    protected virtual void OnEnable()
    {
        CreateSerializableProperties();
        CreateList();
        BuildTypeOptions();
    }
    protected virtual void BuildTypeOptions()
    {
        TypeOptions = ConditionsLoader.AllTypes.Select(x => x.Name).ToArray();
    }
    protected virtual void CreateSerializableProperties()
    {
        Conditions = serializedObject.FindProperty("_conditions");
    }
    protected virtual void CreateList()
    {
        ConditionsList = new ReorderableList(Conditions);
        ConditionsList.onAddCallback += AddCondition;
        ConditionsList.onRemoveCallback += RemoveCondition;
        ConditionsList.drawElementCallback += DrawElement;
        ConditionsList.getElementHeightCallback += GetElementHeight;
    }
}