using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CreateDefaultEnemy
{
    private const string PREFAB_NAME = "Enemy Template";

    [MenuItem("GameObject/Default Enemy", priority = 0)]
    private static void CreateEnemy()
    {
        GameObject prefab = Resources.Load<GameObject>(PREFAB_NAME);
        GameObject instance = GameObject.Instantiate(prefab);

        // Not sure why this is necessary...
        instance.GetComponent<Rigidbody2D>().hideFlags |= HideFlags.HideInInspector;

        instance.name = prefab.name;

        Selection.activeGameObject = instance;
    }
}
