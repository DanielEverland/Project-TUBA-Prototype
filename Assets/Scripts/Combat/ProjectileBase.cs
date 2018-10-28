using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    [SerializeField]
    private MeshRenderer _renderer;

    private float _power;

    public void Initialize(Weapon weapon)
    {
        _power = weapon.TriggerData.Power;
        _renderer.material.color = weapon.SeekerData.Color;
    }
}
