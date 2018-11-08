using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private FloatReference _startHealth;

    private float _health;

    private void Awake()
    {
        _health = _startHealth.Value;
    }
}
