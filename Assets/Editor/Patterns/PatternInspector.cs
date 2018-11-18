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
    protected SerializedProperty Behaviour { get; set; }
    protected SerializedProperty Prefab { get; set; }
    protected ReorderableList List { get; set; }
    protected List<Type> ComponentTypes { get; set; }
    protected string[] ComponentOptions { get; set; }
    protected List<Type> BehaviourTypes { get; set; }
    protected string[] BehaviourOptions { get; set; }

    private const float BEHAVIOUR_PADDING_TOP = 3;
    private const float BEHAVIOUR_PADDING_BOTTOM = 7;
    private const float POPUP_SPACING = 2;

    protected virtual void OnEnable()
    {
        CreateSerializedProperties();
        CreateList();
        BuildAvailablePatterns();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPrefabField();
        DrawSpawnButton();

        EditorGUILayout.Space();

        DrawBehaviour();
        
        using (var scope = new EditorGUI.ChangeCheckScope())
        {
            List.DoLayoutList();

            if (scope.changed)
                EditorUtility.SetDirty(target);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void DrawPrefabField()
    {
        EditorGUILayout.PropertyField(Prefab);
    }
    protected virtual void DrawSpawnButton()
    {
        if (GUILayout.Button("Spawn"))
        {
            Target.Spawn();
        }
    }
    protected virtual void DrawBehaviour()
    {
        DrawBehaviourHeader();

        if(Target.BehaviourFoldoutState)
            DrawBehaviourContent();
    }
    protected virtual void DrawBehaviourHeader()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);

        if (Event.current.type == EventType.Repaint)
            Styles.HeaderBackground.Draw(rect, GUIContent.none, 0);

        using (new EditorGUI.IndentLevelScope())
        {
            rect.y += 1;

            string behaviourName = Target.Behaviour ? Target.Behaviour.GetType().Name : "null";

            Target.BehaviourFoldoutState = EditorGUI.Foldout(rect, Target.BehaviourFoldoutState, new GUIContent($"Behaviour ({behaviourName})"), true);
        }
    }
    protected virtual void DrawBehaviourContent()
    {
        float requiredHeight = 0;
        requiredHeight += EditorGUIUtility.singleLineHeight;
        requiredHeight += BEHAVIOUR_PADDING_TOP;
        requiredHeight += BEHAVIOUR_PADDING_BOTTOM;
        requiredHeight += POPUP_SPACING;

        if (Target.Behaviour != null)
        {
            Type type = Target.Behaviour.GetType();
            PropertyDrawer drawer = GetDrawer(type);

            requiredHeight += drawer.GetPropertyHeight(Behaviour, GUIContent.none);
        }

        Rect rect = EditorGUILayout.GetControlRect(false, requiredHeight);
        
        if(Event.current.type == EventType.Repaint)
            Styles.BoxBackground.Draw(rect, GUIContent.none, 0);

        using (new EditorGUI.IndentLevelScope())
        {
            rect.y += BEHAVIOUR_PADDING_TOP;
            rect.width -= 5;

            DrawBehaviourPopup(rect);

            rect.y += EditorGUIUtility.singleLineHeight + POPUP_SPACING;

            if (Target.Behaviour != null)
            {
                Type type = Target.Behaviour.GetType();
                PropertyDrawer drawer = GetDrawer(type);

                DrawBehaviourInspector(rect, drawer);
            }
        }        
    }
    protected virtual void DrawBehaviourInspector(Rect rect, PropertyDrawer drawer)
    {
        if (Target.Behaviour == null)
            return;
        
        drawer.OnGUI(rect, Behaviour, GUIContent.none);
    }
    protected virtual void DrawBehaviourPopup(Rect rect)
    {
        Type type = Target.Behaviour?.GetType();
        int optionIndex = type == null ? BehaviourOptions.Length - 1 : BehaviourTypes.IndexOf(type);

        rect.height = EditorGUIUtility.singleLineHeight;
        int newIndex = EditorGUI.Popup(rect, optionIndex, BehaviourOptions);

        if (newIndex != optionIndex)
            SwitchBehaviourType(newIndex);

        void SwitchBehaviourType(int typeIndex)
        {
            if(typeIndex >= BehaviourTypes.Count)
            {
                RemoveObject(Target.Behaviour);
                Target.Behaviour = null;
            }
            else
            {
                if (Target.Behaviour != null)
                    RemoveObject(Target.Behaviour);

                PatternBehaviour newInstance = CreateBehaviour(typeIndex);

                Target.Behaviour = newInstance;
                AddObject(newInstance);
            }
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
        inspectorRect = new Rect(rect.x, rect.y + rect.height, rect.width, requiredHeight);

        drawer.OnGUI(inspectorRect, element, GUIContent.none);
    }
    protected virtual void DrawPopup(Rect rect, out Rect popupRect, int elementIndex)
    {
        Type type = Target.Components[elementIndex].GetType();

        popupRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight + POPUP_SPACING);
        int optionIndex = ComponentTypes.IndexOf(type);

        int newIndex = EditorGUI.Popup(popupRect, optionIndex, ComponentOptions);

        if (newIndex != optionIndex)
            SwitchInstanceType(newIndex);

        void SwitchInstanceType(int typeIndex)
        {
            RemoveObject(Target.Components[elementIndex]);

            PatternComponent newInstance = CreateComponent(typeIndex);

            Target.Components[elementIndex] = newInstance;
            AddObject(newInstance);
        }
    }
    protected virtual void RemoveObject(Object obj)
    {
        DestroyImmediate(obj, true);
    }
    protected virtual void AddObject(Object obj)
    {
        obj.name = obj.GetType().Name;

        AssetDatabase.AddObjectToAsset(obj, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
        AssetDatabase.SaveAssets();
    }
    protected virtual PatternComponent GetObject(SerializedProperty element)
    {
        int index = List.IndexOf(element);

        return Target.Components[index];
    }
    protected virtual void AddComponent(ReorderableList list)
    {
        PatternComponent newComponent = CreateComponent(0);
        Target.Components.Add(newComponent);
    }
    protected virtual PatternComponent CreateComponent(int index)
    {
        return CreateComponent(ComponentTypes[index]);
    }
    protected virtual PatternComponent CreateComponent(Type type)
    {
        return (PatternComponent)ScriptableObject.CreateInstance(type);
    }
    protected virtual PatternBehaviour CreateBehaviour(int index)
    {
        return CreateBehaviour(BehaviourTypes[index]);
    }
    protected virtual PatternBehaviour CreateBehaviour(Type type)
    {
        return (PatternBehaviour)ScriptableObject.CreateInstance(type);
    }
    protected virtual void BuildAvailablePatterns()
    {
        ComponentTypes = new List<Type>(PatternLoader.Components);
        ComponentOptions = new List<string>(PatternLoader.Components.Select(x => x.Name)).ToArray();

        BehaviourTypes = new List<Type>(PatternLoader.Behaviours);

        List<string> options = new List<string>(PatternLoader.Behaviours.Select(x => x.Name));
        options.Add("null");

        BehaviourOptions = options.ToArray();
    }
    protected virtual PropertyDrawer GetDrawer(Type type)
    {
        if (!PropertyDrawerLoader.Drawers.ContainsKey(type))
            throw new System.NotImplementedException($"No PropertyDrawer for ({type.Name})");

        return PropertyDrawerLoader.Drawers[type];
    }
    protected virtual float GetElementHeight(SerializedProperty property)
    {
        int elementIndex = List.IndexOf(property);
        PropertyDrawer drawer = GetDrawer(Target.Components[elementIndex].GetType());

        return drawer.GetPropertyHeight(property, GUIContent.none) + EditorGUIUtility.singleLineHeight + POPUP_SPACING;
    }
    protected virtual void CreateSerializedProperties()
    {
        Behaviour = serializedObject.FindProperty("_behaviour");
        Components = serializedObject.FindProperty("_components");
        Prefab = serializedObject.FindProperty("_prefab");
    }
    protected virtual void CreateList()
    {
        List = new ReorderableList(Components);
        List.drawElementCallback += DrawElement;
        List.onAddCallback += AddComponent;
        List.getElementHeightCallback += GetElementHeight;
    }

    private static class Styles
    {
        public static GUIStyle BoxBackground;
        public static GUIStyle HeaderBackground;

        static Styles()
        {
            BoxBackground = new GUIStyle("RL Background");
            HeaderBackground = new GUIStyle("RL Header");
        }
    }
}
