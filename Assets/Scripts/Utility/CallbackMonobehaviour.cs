using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CallbackMonobehaviour : MonoBehaviour {

    [SerializeField, EnumFlags]
    private CallbackState _callbackState;

    protected abstract void OnRaised();

    private void Awake()
    {
        if (_callbackState.HasFlag(CallbackState.Awake))
            OnRaised();
    }
    private void Start()
    {
        if (_callbackState.HasFlag(CallbackState.Start))
            OnRaised();
    }
    private void Update()
    {
        if (_callbackState.HasFlag(CallbackState.Update))
            OnRaised();
    }
}
