using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableResetter : MonoBehaviour
{
    [SerializeField]
    private BaseVariable _target;
    [SerializeField]
    private BaseVariable _source;
    [SerializeField]
    private CallbackState _resetCallback = CallbackState.Awake;

    private void Awake()
    {
        if (_resetCallback == CallbackState.Awake)
            DoReset();
    }
    private void Start()
    {
        if (_resetCallback == CallbackState.Start)
            DoReset();
    }
    private void DoReset() => _target.BaseValue = _source.BaseValue;

    [System.Serializable]
    public enum CallbackState
    {
        Awake,
        Start,
    }
}