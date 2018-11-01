using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAngler : MonoBehaviour, IAttackHandlerComponent
{
    [SerializeField]
    private Vector3Reference _directionVariable;
    [SerializeField]
    private Transform _weaponTransform;
    
    public void Poll(WeaponInputResponse input)
    {
        Vector3 direction = _directionVariable.Value.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _weaponTransform.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}