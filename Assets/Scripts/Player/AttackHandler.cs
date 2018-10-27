using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour {

    [SerializeField]
    private GameEvent _onFire;
    [SerializeField]
    private WeaponVariable _selectedWeapon;
    [SerializeField]
    private Transform _weaponTransform;
    [SerializeField]
    private FloatReference _chargeTime;

    private bool CanFire
    {
        get
        {
            return Time.time - _lastFireTime > _selectedWeapon.Value.TriggerData.Cooldown;
        }        
    }

    private float _lastFireTime = float.MinValue;
    private float? _fireDownTime = null;

    private void Update()
    {
        InputResponse response = PollInput();

        HandleCharge(response);
        
        if (response.HasInput)
        {
            Vector3 targetPosition = transform.position + (Vector3)(response.InputDirection.normalized * 10);

            Debug.DrawLine(transform.position, targetPosition, Color.red);

            AngleWeapon(response);

            if (CanFire)
            {
                Fire();
            }
        }
        else
        {
            _fireDownTime = null;
        }
    }
    private void HandleCharge(InputResponse response)
    {
        if (!_selectedWeapon.Value.TriggerData.UseCharge)
        {
            _chargeTime.Value = 0;
            return;
        }
        else if (response.HasInput)
        {
            float desiredValue = response.HasInput ? Time.time - _fireDownTime.Value : 0;
            _chargeTime.Value = Mathf.Clamp(desiredValue, 0, _selectedWeapon.Value.TriggerData.ChargeTime);
        }
        else
        {
            _chargeTime.Value = 0;
        }
    }
    private void AngleWeapon(InputResponse response)
    {
        Vector3 direction = response.InputDirection.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _weaponTransform.transform.eulerAngles = new Vector3(0, 0, angle);
    }
    private void Fire()
    {
        _lastFireTime = Time.time;

        _onFire.Raise();
    }
    private InputResponse PollInput()
    {
        InputResponse response = default(InputResponse);

        PollControllerInput(ref response);
        PollMouseInput(ref response);

        if (response.HasInput && _fireDownTime == null)
            _fireDownTime = Time.time;

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
            Vector2 playerScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 mouseDelta = (Vector2)Input.mousePosition - playerScreenSpace;

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
