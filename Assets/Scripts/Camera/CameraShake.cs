using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField]
    private float _duration = 0.2f;
    [SerializeField]
    private float _shakeIntensity = 1;

    protected float Duration => _duration;
    protected float Force => _shakeIntensity;
    protected bool IsShaking => Time.time - _currentEntryStartTime < Duration;    
    
    private float _currentEntryStartTime;
    
    public void AddForce()
    {
        _currentEntryStartTime = Time.time;
    }
    private void LateUpdate()
    {
        if(IsShaking)
        {
            transform.position += Random.insideUnitSphere * Force * Time.deltaTime;
        }            
    }
}
