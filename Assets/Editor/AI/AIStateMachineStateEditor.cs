using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(AIStateMachineStateNode))]
public class AIStateMachineStateEditor : Editor
{
    protected AIStateMachineStateNode Target { get { return (AIStateMachineStateNode)target; } }
    protected SerializedProperty Actions { get; set; }
    protected SerializedProperty StartEvent { get; set; }
    protected SerializedProperty EndEvent { get; set; }
    protected SerializedProperty Transitions { get; set; }    
    protected ReorderableList ActionsList { get; set; }
    protected ReorderableList TransitionsList { get; set; }
    protected string[] TypeOptions { get; set; }

    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();
        
        target.name = EditorGUILayoutHelper.DrawHeaderTextField(target.name);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        ActionsList.DoLayoutList();
        
        DrawEvents();

        TransitionsList.isExpanded = Target.TransitionsFoldout;
        TransitionsList.DoLayoutList();
        Target.TransitionsFoldout = TransitionsList.isExpanded;

        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void DrawEvents()
    {
        EditorGUILayout.PropertyField(StartEvent);
        EditorGUILayout.PropertyField(EndEvent);
    }
    protected virtual void DrawElement(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        int index = ActionsList.IndexOf(element);
        AIStateMachineAction action = Target.Actions[index];
        System.Type type = action.GetType();
        int typeIndex = ActionLoader.AllTypes.IndexOf(type);

        rect.height = EditorGUIUtility.singleLineHeight;

        int newIndex = EditorGUI.Popup(rect, typeIndex, TypeOptions);

        rect.y += EditorGUIUtility.singleLineHeight;

        GenericPropertyDrawer.Drawer.OnGUI(rect, element, GUIContent.none);

        if (newIndex != typeIndex)
        {
            RemoveObject(action);
            Target.Actions.Remove(action);

            AIStateMachineAction newAction = CreateNewAction(ActionLoader.AllTypes[newIndex]);
            Target.Actions.Insert(index, newAction);

            AddObject(newAction);
        }
    }
    protected virtual void AddAction(ReorderableList list)
    {
        AIStateMachineAction newAction = CreateNewAction(ActionLoader.AllTypes[0]);
        Target.Actions.Add(newAction);

        AddObject(newAction);
    }
    protected virtual AIStateMachineAction CreateNewAction(System.Type type)
    {
        AIStateMachineAction newAction = (AIStateMachineAction)ScriptableObject.CreateInstance(type);

        newAction.name = newAction.GetType().Name;

        return newAction;
    }
    protected virtual void AddObject(Object obj)
    {
        AssetDatabase.AddObjectToAsset(obj, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
    }
    protected virtual void RemoveAction(ReorderableList list)
    {
        AIStateMachineAction toRemove = Actions.GetArrayElementAtIndex(list.Index).objectReferenceValue as AIStateMachineAction;

        Target.Actions.Remove(toRemove);
        RemoveObject(toRemove);
    }
    protected virtual void RemoveObject(Object obj)
    {
        DestroyImmediate(obj, true);
    }
    protected virtual float GetElementHeight(SerializedProperty element)
    {
        int index = ActionsList.IndexOf(element);
        return GenericPropertyDrawer.Drawer.GetPropertyHeight(element, GUIContent.none) + EditorGUIUtility.singleLineHeight;
    }
    protected virtual void OnEnable()
    {
        CreateSerializableProperties();
        CreateActionList();
        CreateTransitionsList();
        BuildTypeOptions();
    }
    protected virtual void BuildTypeOptions()
    {
        TypeOptions = ActionLoader.AllTypes.Select(x => x.Name).ToArray();
    }
    protected virtual void CreateSerializableProperties()
    {
        Actions = serializedObject.FindProperty("_actions");
        Transitions = serializedObject.FindProperty("_transitions");
        StartEvent = serializedObject.FindProperty("_onStateStarted");
        EndEvent = serializedObject.FindProperty("_onStateEnded");
    }
    protected virtual void CreateActionList()
    {
        ActionsList = new ReorderableList(Actions);
        ActionsList.onAddCallback += AddAction;
        ActionsList.onRemoveCallback += RemoveAction;
        ActionsList.drawElementCallback += DrawElement;
        ActionsList.getElementHeightCallback += GetElementHeight;
    }
    protected virtual void CreateTransitionsList()
    {
        TransitionsList = new ReorderableList(Transitions, false, false, true);
    }
}
