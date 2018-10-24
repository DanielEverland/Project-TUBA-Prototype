using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private FloatReference _maxHealth;
    [SerializeField]
    private FloatReference _currentHealth;

    private void Awake()
    {
        _currentHealth.Value = _maxHealth.Value;
    }
}
