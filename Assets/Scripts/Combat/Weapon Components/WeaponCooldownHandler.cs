using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCooldownHandler : MonoBehaviour
{
    [SerializeField]
    private FloatReference _cooldownTimeProperty;
    [SerializeField]
    private FloatReference _currentCooldown;
    [SerializeField]
    private FloatReference _lastFireTime;

    private float LastFireTime => _lastFireTime.Value;
    private float CooldownTime => _cooldownTimeProperty.Value;
    private float CurrentCooldown { get { return _currentCooldown.Value; } set { _currentCooldown.Value = value; } }
        
    public void CalculateCooldown() => CurrentCooldown = Mathf.Clamp(Time.time - LastFireTime, 0, CooldownTime);
}