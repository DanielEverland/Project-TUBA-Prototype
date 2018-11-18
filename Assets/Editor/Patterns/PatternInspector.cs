using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

[CustomEditor(typeof(Pattern))]
public class PatternInspector : Editor
{
    protected Pattern Target => (Pattern)target;
    protected SerializedProperty Components { get; set; }
    protected ReorderableList List { get; set; }
    protected List<Type> PatternTypes { get; set; }
    protected string[] PatternOptions { get; set; }

    protected virtual void OnEnable()
    {
        CreateSerializedProperties();
        CreateList();
        BuildAvailablePatterns();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawSpawnButton();

        EditorGUILayout.Space();

        using (var scope = new EditorGUI.ChangeCheckScope())
        {
            List.DoLayoutList();

            if (scope.changed)
                EditorUtility.SetDirty(target);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void DrawSpawnButton()
    {
        if (GUILayout.Button("Spawn"))
        {
            Target.Spawn();
        }
    }
    protected virtual void DrawElement(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        int elementIndex = List.IndexOf(element);

        DrawPopup(rect, out Rect popupRect, elementIndex);
        DrawInspector(popupRect, out Rect inspectorRect, elementIndex, element);
    }
    protected virtual void DrawInspector(Rect rect, out Rect inspectorRect, int elementIndex, SerializedProperty element)
    {
        PatternComponent currentComponent = Target.Components[elementIndex];
        Type type = currentComponent.GetType();
        
        PropertyDrawer drawer = GetDrawer(type);
        float requiredHeight = drawer.GetPropertyHeight(element, GUIContent.none);
        inspectorRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, requiredHeight);

        drawer.OnGUI(inspectorRect, element, GUIContent.none);
    }
    protected virtual void DrawPopup(Rect rect, out Rect popupRect, int elementIndex)
    {
        Type type = Target.Components[elementIndex].GetType();

        popupRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
        int optionIndex = PatternTypes.IndexOf(type);

        int newIndex = EditorGUI.Popup(popupRect, optionIndex, PatternOptions);

        if (newIndex != optionIndex)
            SwitchInstanceType(newIndex);

        void SwitchInstanceType(int typeIndex)
        {
            PatternComponent newInstance = CreateInstance(typeIndex);

            Target.Components[elementIndex] = newInstance;
        }
    }
    protected virtual PatternComponent GetObject(SerializedProperty element)
    {
        int index = List.IndexOf(element);

        return Target.Components[index];
    }
    protected virtual void AddComponent(ReorderableList list)
    {
        PatternComponent newComponent = CreateInstance(0);
        Target.Components.Add(newComponent);
    }
    protected virtual PatternComponent CreateInstance(int index)
    {
        return CreateInstance(PatternTypes[index]);
    }
    protected new virtual PatternComponent CreateInstance(Type type)
    {
        return (PatternComponent)ScriptableObject.CreateInstance(type);
    }
    protected virtual void BuildAvailablePatterns()
    {
        PatternTypes = new List<Type>(PatternLoader.Patterns);
        PatternOptions = new List<string>(PatternLoader.Patterns.Select(x => x.Name)).ToArray();
    }
    protected virtual PropertyDrawer GetDrawer(Type type)
    {
        if (!PatternLoader.Drawers.ContainsKey(type))
            throw new System.NotImplementedException($"No PropertyDrawer for ({type.Name})");

        return PatternLoader.Drawers[type];
    }
    protected virtual float GetElementHeight(SerializedProperty property)
    {
        int elementIndex = List.IndexOf(property);
        PropertyDrawer drawer = GetDrawer(Target.Components[elementIndex].GetType());

        return drawer.GetPropertyHeight(property, GUIContent.none) + EditorGUIUtility.singleLineHeight;
    }
    protected virtual void CreateSerializedProperties()
    {
        Components = serializedObject.FindProperty("_components");
    }
    protected virtual void CreateList()
    {
        List = new ReorderableList(Components);
        List.drawElementCallback += DrawElement;
        List.onAddCallback += AddComponent;
        List.getElementHeightCallback += GetElementHeight;
    }
}
