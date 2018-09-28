using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementToggle : MonoBehaviour {
    
    [SerializeField]
    private bool _autoSetStartState;
    [SerializeField]
    private bool _startState;
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private KeyCode _keyCode;
    
    private void Awake()
    {
        if (_autoSetStartState)
            SetState(_startState);
    }
    private void Update()
    {
        if (Input.GetKeyUp(_keyCode))
        {
            try
            {
                SetState(!_target.activeInHierarchy);
            }
            catch (System.NullReferenceException)
            {
                if (_target == null)
                {
                    throw new System.NullReferenceException("Element Toggle Error: Target cannot be null");
                }

                throw;
            }
        }
    }
    private void EnsureTargetIsNotSelf()
    {
        if(_target == gameObject)
        {
            Debug.LogError("Element Toggle Error: Target cannot be self. This will disallow event capture when disabled", this);
        }
    }
    private void SetState(bool active)
    {
        try
        {
            _target.SetActive(active);
        }
        catch (System.NullReferenceException)
        {
            if(_target == null)
            {
                throw new System.NullReferenceException("Element Toggle Error: Target cannot be null");
            }

            throw;
        }
    }
}
