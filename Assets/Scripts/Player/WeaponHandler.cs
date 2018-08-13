using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    [SerializeField]
    private GameEvent OnFire;

    private void Update()
    {
        InputResponse response = PollInput();

        if (response.HasInput)
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3)(response.InputDirection.normalized * 10), Color.red);

            OnFire.Raise();
        }        
    }
    private InputResponse PollInput()
    {
        InputResponse response = default(InputResponse);

        PollControllerInput(ref response);
        PollMouseInput(ref response);

        return response;
    }
    private void PollControllerInput(ref InputResponse response)
    {
        Vector2 rightAnalogue = new Vector2()
        {
            x = Input.GetAxis("Right Horizontal"),
            y = Input.GetAxis("Right Vertical"),
        };

        if(rightAnalogue != default(Vector2))
        {
            response.InputDirection = rightAnalogue.normalized;
        }
    }
    private void PollMouseInput(ref InputResponse response)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 mouseDelta = (Vector2)Input.mousePosition - screenCenter;

            response.InputDirection = mouseDelta.normalized;
        }
    }

    private struct InputResponse
    {
        public bool HasInput { get { return _hasInput; } }
        public Vector2 InputDirection
        {
            get
            {
                return _inputDirection;
            }
            set
            {
                _inputDirection = value;
                _hasInput = true;
            }
        }

        private bool _hasInput;
        private Vector2 _inputDirection;

        public static implicit operator bool(InputResponse response)
        {
            return response.HasInput;
        }
    }
}
