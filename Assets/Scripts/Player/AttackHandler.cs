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
    private FloatReference _weaponChargeTime;
    [SerializeField]
    private FloatReference _currentCharge;
    [SerializeField]
    private FloatReference _cooldownTime;
    [SerializeField]
    private FloatReference _currentCooldown;
    [SerializeField]
    private BoolReference _useCharge;
    [SerializeField]
    private IntReference _currentAmmoCount;
    [SerializeField]
    private IntReference _maxAmmoCount;
    [SerializeField]
    private Transform _weaponDirection;
    [SerializeField]
    private ProjectileBase _projectile;

    private bool UseCharge { get { return _useCharge.Value; } }
    private float CurrentCooldown { get { return _currentCooldown.Value; } set { _currentCooldown.Value = value; } }
    private float CurrentCharge { get { return _currentCharge.Value; } set { _currentCharge.Value = value; } }
    private float CooldownTime { get { return _cooldownTime.Value; } }
    private float ChargeTime { get { return _weaponChargeTime.Value; } }

    private bool OnCooldown
    {
        get
        {
            return CurrentCooldown < CooldownTime;
        }
    }
    public int CurrentAmmo
    {
        get
        {
            return _selectedWeapon.Value.CurrentAmmo;
        }
        set
        {
            _selectedWeapon.Value.CurrentAmmo = value;
            _currentAmmoCount.Value = _selectedWeapon.Value.CurrentAmmo;
        }
    }

    private float _lastFireTime = float.MinValue;
    private float? _fireDownTime = null;
    private Vector2 _direction = default(Vector2);
    private InputResponse _previousResponse = default(InputResponse);

    private const float DEBUG_RAY_LENGTH = 3;

    private void Update()
    {
        InputResponse response = PollInput();
                
        PollWeaponFire(response);
        ToggleFireDown(response);
        AngleWeapon(response);

        _previousResponse = response;
    }
    private void LateUpdate()
    {
        DrawDebug();
    }
    private void DrawDebug()
    {
        Debug.DrawRay(transform.position, _direction * DEBUG_RAY_LENGTH, Color.cyan);
    }
    private void PollWeaponFire(InputResponse response)
    {
        CalculateCooldown();
        PollFireWeapon(response);
    }    
    private void PollFireWeapon(InputResponse response)
    {        
        if(CurrentCharge >= ChargeTime && !OnCooldown && IsFireButtonPressed(response))
        {
            Fire();
        }

        if (response.FireButtonDown && !OnCooldown)
        {
            ChargeWeapon();
        }
        else
        {
            ResetCharge();
        }
    }
    private bool IsFireButtonPressed(InputResponse response)
    {
        if (UseCharge)
        {
            return response.FireButtonUp;
        }
        else
        {
            return response.FireButtonDown;
        }
    }
    private void CalculateCooldown()
    {
        CurrentCooldown = Mathf.Clamp(Time.time - _lastFireTime, 0, CooldownTime);
    }
    private void ChargeWeapon()
    {
        float desiredCharge = CurrentCharge + Time.deltaTime;
        CurrentCharge = Mathf.Clamp(desiredCharge, 0, ChargeTime);
    }
    private void ToggleFireDown(InputResponse response)
    {
        if (response.FireButtonDown && _fireDownTime == null)
        {
            _fireDownTime = Time.time;
        }
        else
        {
            _fireDownTime = null;
        }
    }
    private InputResponse PollInput()
    {
        InputResponse response = InputResponse.Create(_previousResponse, gameObject);

        if (response.FireButtonDown && _fireDownTime == null)
            _fireDownTime = Time.time;

        if (response.HasDirection)
        {
            _direction = response.InputDirection;
        }            

        return response;
    }
    private void AngleWeapon(InputResponse response)
    {
        Vector3 direction = _direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _weaponTransform.transform.eulerAngles = new Vector3(0, 0, angle);
    }
    private void ResetCharge()
    {
        CurrentCharge = 0;
    }
    private void Fire()
    {
        _lastFireTime = Time.time;

        CurrentAmmo = Mathf.Clamp(CurrentAmmo - 1, 0, _maxAmmoCount.Value);

        SpawnProjectile();

        _onFire.Raise();
    }
    private void SpawnProjectile()
    {
        ProjectileBase instance = Instantiate(_projectile);
        instance.Initialize(_selectedWeapon.Value);

        instance.transform.position = _weaponDirection.position;
        instance.transform.rotation = _weaponDirection.rotation;
    }
    

    private struct InputResponse
    {
        public bool FireButtonDown { get { return _controllerFireButtonDown || _keyboardFireButtonDown; } }
        public bool FireButtonUp { get { return _controllerFireButtonUp || _keyboardFireButtonUp; } }
        public bool HasDirection { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public Vector2 InputDirection
        {
            get
            {
                return _inputDirection;
            }
            set
            {
                _inputDirection = value;
                HasDirection = true;
            }
        }

        private Vector2 _inputDirection;
        private bool _controllerFireButtonDown;
        private bool _controllerFireButtonUp;
        private bool _keyboardFireButtonDown;
        private bool _keyboardFireButtonUp;

        private const KeyCode FIRE_BUTTON_KEYBOARD = KeyCode.Mouse0;

        public static InputResponse Create(InputResponse previous, GameObject player)
        {
            InputResponse response = default(InputResponse);

            response.PollControllerInput(previous);
            response.PollMouseInput(previous, player);

            return response;
        }
        private void PollControllerInput(InputResponse previous)
        {
            // Direction.
            Vector2 rightAnalogue = new Vector2()
            {
                x = Input.GetAxis("Right Horizontal"),
                y = Input.GetAxis("Right Vertical"),
            };

            if (rightAnalogue != default(Vector2))
            {
                InputDirection = rightAnalogue.normalized;
            }

            // Fire button.
            if (ControllerFireButtonDown())
                _controllerFireButtonDown = true;

            if(previous._controllerFireButtonDown && !_controllerFireButtonDown)
                _controllerFireButtonUp = true;
        }
        private void PollMouseInput(InputResponse previous, GameObject player)
        {
            MousePosition = Input.mousePosition;

            // Fire button.
            if(Input.GetKey(FIRE_BUTTON_KEYBOARD) || Input.GetKeyDown(FIRE_BUTTON_KEYBOARD))
                _keyboardFireButtonDown = true;

            if(Input.GetKeyUp(FIRE_BUTTON_KEYBOARD))
                _keyboardFireButtonUp = true;

            // Direction.
            if (MousePosition != previous.MousePosition)
            {
                Vector2 playerScreenSpace = Camera.main.WorldToScreenPoint(player.transform.position);
                Vector2 mouseDelta = (Vector2)Input.mousePosition - playerScreenSpace;

                InputDirection = mouseDelta.normalized;
            }
        }
        private static bool ControllerFireButtonDown()
        {
            return Input.GetAxis("Right Trigger") > 0;
        }
    }
}
