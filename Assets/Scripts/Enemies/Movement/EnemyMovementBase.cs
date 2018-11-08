using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBase : MonoBehaviour
{
    [SerializeField]
    private FloatReference _movementSpeed;
    [SerializeField]
    private MovementPostProcessor _postProcessor;

    protected float MovementSpeed => GetMovementSpeed();

    private float GetMovementSpeed()
    {
        if (_postProcessor != null)
            return _postProcessor.ProcessMovementSpeed(_movementSpeed.Value);

        return _movementSpeed.Value;
    }
    protected virtual void OnValidate()
    {
        if (_postProcessor == null)
            _postProcessor = GetComponent<MovementPostProcessor>();
    }
}
