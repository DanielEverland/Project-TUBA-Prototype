﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AITransition))]
public class AITransitionEditor : Editor
{
    protected AITransition Target { get { return (AITransition)target; } }
    protected SerializedProperty Conditions { get; set; }
    protected SerializedProperty TransitionTime { get; set; }
    protected ReorderableList ConditionsList { get; set; }
    protected string[] TypeOptions { get; set; }

    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();

        target.name = EditorGUILayoutHelper.DrawHeaderTextField(target.name);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(TransitionTime);

        ConditionsList.DoLayoutList();
        
        DrawSelectButton();

        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void DrawSelectButton()
    {
        if(Application.isPlaying)
        {
            if (GUILayout.Button("Force Current"))
            {
                AIStateMachineWindow.Agent.ChangeCurrentObject(Target);
            }
        }
    }
    protected virtual void DrawElement(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        int index = ConditionsList.IndexOf(element);
        AICondition condition = Target.Conditions[index];
        System.Type type = condition.GetType();
        int typeIndex = ConditionsLoader.AllTypes.IndexOf(type);

        rect.height = EditorGUIUtility.singleLineHeight;
        
        int newIndex = EditorGUI.Popup(rect, typeIndex, TypeOptions);

        rect.y += EditorGUIUtility.singleLineHeight;

        GenericPropertyDrawer.Drawer.OnGUI(rect, element, GUIContent.none);

        if(newIndex != typeIndex)
        {
            RemoveObject(condition);
            Target.Conditions.Remove(condition);

            AICondition newCondition = CreateNewCondition(ConditionsLoader.AllTypes[newIndex]);
            Target.Conditions.Insert(index, newCondition);

            AddObject(newCondition);
        }
    }
    protected virtual void AddCondition(ReorderableList list)
    {
        AICondition newCondition = CreateNewCondition(ConditionsLoader.AllTypes[0]);
        Target.Conditions.Add(newCondition);

        AddObject(newCondition);
    }
    protected virtual AICondition CreateNewCondition(System.Type type)
    {
        AICondition newCondition = (AICondition)ScriptableObject.CreateInstance(type);

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
        AICondition toRemove = Conditions.GetArrayElementAtIndex(list.Index).objectReferenceValue as AICondition;

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
        return GenericPropertyDrawer.Drawer.GetPropertyHeight(element, GUIContent.none) + EditorGUIUtility.singleLineHeight;
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
        TransitionTime = serializedObject.FindProperty("_transitionTime");
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
