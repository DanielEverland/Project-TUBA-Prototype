using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(ElementToggle))]
[CanEditMultipleObjects]
public class ElementToggleEditor : Editor {

    private SerializedProperty _targetProperty;
    private SerializedProperty _autoSetStartState;
    private SerializedProperty _startState;
    private SerializedProperty _keyCode;

    private AnimBool _startStateAnim;

    private void OnEnable()
    {
        _targetProperty = serializedObject.FindProperty("_target");
        _autoSetStartState = serializedObject.FindProperty("_autoSetStartState");
        _startState = serializedObject.FindProperty("_startState");
        _keyCode = serializedObject.FindProperty("_keyCode");

        _startStateAnim = new AnimBool(_autoSetStartState.boolValue);
        _startStateAnim.valueChanged.AddListener(Repaint);
    }
    public override void OnInspectorGUI()
    {
        //Target
        EditorGUILayout.ObjectField(_targetProperty);

        if(_targetProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Target cannot be null", MessageType.Error);
        }
        else if(_targetProperty.objectReferenceValue == ((MonoBehaviour)target).gameObject)
        {
            EditorGUILayout.HelpBox("Target cannot be self\nThis will disallow key capture when target is disabled", MessageType.Warning);
        }

        //Keycode
        EditorGUILayout.PropertyField(_keyCode);

        //Auto-set Start State
        EditorGUILayout.PropertyField(_autoSetStartState);
        _startStateAnim.target = _autoSetStartState.boolValue;
        
        using (var group = new EditorGUILayout.FadeGroupScope(_startStateAnim.faded))
        {
            if (group.visible)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(_startState);

                EditorGUI.indentLevel--;
            }
        }
        
        serializedObject.ApplyModifiedProperties();        
    }
}
