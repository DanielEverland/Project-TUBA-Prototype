using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIStateMachineStartNode))]
public class AIStateMachineStartNodeEditor : Editor
{
    AIStateMachineStartNode Target => target as AIStateMachineStartNode;

    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();

        target.name = EditorGUILayoutHelper.DrawHeaderTextField(target.name);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Start Node has no behaviour", MessageType.Info);

        DrawSelectButton();
    }
    protected virtual void DrawSelectButton()
    {
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Force Current"))
            {
                Target.Machine.ChangeCurrentObject(Target);
            }
        }
    }
}
