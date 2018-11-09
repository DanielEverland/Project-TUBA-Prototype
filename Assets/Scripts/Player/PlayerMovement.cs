using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private CharacterController2D _characterController;
    [SerializeField]
    private FloatReference _movementSpeed;
    
    private void Update()
    {
        Vector2 input = PollInput();
        Move(input);
    }
    private void Move(Vector2 direction)
    {
        _characterController.Move(direction * _movementSpeed.Value * Time.deltaTime);
    }
    private Vector2 PollInput()
    {
        return new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical"),
        }.normalized;
    }
    private void OnValidate()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController2D>();
    }
}
