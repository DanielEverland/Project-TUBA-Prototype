using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private bool _alignPosition = true;
    [SerializeField]
    private bool _alignRotation = true;
    [SerializeField]
    private GameObject _prefab;

    public void Spawn()
    {
        GameObject instance = GameObject.Instantiate(_prefab);

        if (_alignPosition)
            instance.transform.position = transform.position;

        if (_alignRotation)
            instance.transform.rotation = transform.rotation;
    }
}