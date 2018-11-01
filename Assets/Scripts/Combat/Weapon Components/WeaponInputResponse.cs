using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponInputResponse
{
    public bool ReloadButtonDown { get; private set; }
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
    private const KeyCode RELOAD_BUTTON_KEYBOARD = KeyCode.R;
    private const KeyCode RELOAD_BUTTON_CONTROLLER = KeyCode.Joystick1Button2;

    public static WeaponInputResponse Create(WeaponInputResponse previous, GameObject player)
    {
        WeaponInputResponse response = default(WeaponInputResponse);

        response.PollMouseInput();
        response.PollControllerFireButton(previous);
        response.PollControllerDirection();
        response.PollKeyboardFireButton();
        response.PollReloadButton();
        
        //Should be after fire button polls
        response.PollMouseDirection(previous, player);

        return response;
    }
    private void PollMouseInput()
    {
        MousePosition = Input.mousePosition;
    }
    private void PollControllerFireButton(WeaponInputResponse previous)
    {
        if (ControllerFireButtonDown())
            _controllerFireButtonDown = true;

        if (previous._controllerFireButtonDown && !_controllerFireButtonDown)
            _controllerFireButtonUp = true;
    }
    private void PollKeyboardFireButton()
    {
        if (Input.GetKey(FIRE_BUTTON_KEYBOARD) || Input.GetKeyDown(FIRE_BUTTON_KEYBOARD))
            _keyboardFireButtonDown = true;

        if (Input.GetKeyUp(FIRE_BUTTON_KEYBOARD))
            _keyboardFireButtonUp = true;
    }
    private void PollReloadButton()
    {
        if (Input.GetKeyDown(RELOAD_BUTTON_KEYBOARD) || Input.GetKeyDown(RELOAD_BUTTON_CONTROLLER))
            ReloadButtonDown = true;
    }
    private void PollControllerDirection()
    {
        Vector2 rightAnalogue = new Vector2()
        {
            x = Input.GetAxis("Right Horizontal"),
            y = Input.GetAxis("Right Vertical"),
        };

        if (rightAnalogue != default(Vector2))
        {
            InputDirection = rightAnalogue.normalized;
        }
    }
    private void PollMouseDirection(WeaponInputResponse previous, GameObject player)
    {
        if (MousePosition != previous.MousePosition || FireButtonDown)
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