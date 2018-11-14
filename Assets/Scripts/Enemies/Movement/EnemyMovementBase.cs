using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovementBase : AIComponent
{
    [SerializeField]
    private CharacterController2D _characterController;
    [SerializeField]
    private FloatReference _movementSpeed = new FloatReference(1);
    [SerializeField, HideInInspector]
    private MovementPostProcessor _postProcessor;

    protected CharacterController2D CharacterController => _characterController;
    protected float MovementSpeed => GetMovementSpeed();

    private float GetMovementSpeed()
    {
        if (_postProcessor != null)
            return _postProcessor.ProcessMovementSpeed(_movementSpeed.Value);

        return _movementSpeed.Value;
    }
    protected virtual void OnValidate()
    {
        _postProcessor = GetComponent<MovementPostProcessor>();

        if (_characterController == null)
            _characterController = GetComponent<CharacterController2D>();
    }
}
