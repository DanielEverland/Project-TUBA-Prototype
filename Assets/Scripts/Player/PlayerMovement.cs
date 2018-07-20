using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private FloatReference _movementSpeed;

    private void Update()
    {
        Vector2 input = PollInput();
        Move(input);
    }
    private void Move(Vector2 direction)
    {
        _characterController.Move(direction * _movementSpeed * Time.deltaTime);
    }
    private Vector2 PollInput()
    {
        return new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical"),
        }.normalized;
    }
}
