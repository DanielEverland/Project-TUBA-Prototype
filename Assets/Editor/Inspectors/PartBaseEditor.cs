using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartBase), true)]
public class PartBaseEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string path = AssetDatabase.GetAssetPath(target);

        if(!path.Contains("Resources"))
        {
            EditorGUILayout.HelpBox("Asset is not in Resources folder", MessageType.Warning);
        }        
    }
}
