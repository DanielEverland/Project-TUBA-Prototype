using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerRumble : MonoBehaviour
{
    private PlayerIndex _playerIndex;
    private GamePadState _state;
    private GamePadState _prevState;

    private float _vibrationStart;
    private float _vibrationDuration;

    private bool ShouldVibrate => Time.unscaledTime - _vibrationStart < _vibrationDuration;
    
    public void Vibrate(float duration)
    {
        _vibrationStart = Time.unscaledTime;
        _vibrationDuration = duration;
    }
    private void Update()
    {
        PollControllerConnection();
        SetState();
        PollRumble();
    }
    private void SetState()
    {
        _prevState = _state;
        _state = GamePad.GetState(_playerIndex);
    }
    private void PollRumble()
    {
        float amount = ShouldVibrate ? 1 : 0;
        GamePad.SetVibration(_playerIndex, amount, amount);
    }
    private void PollControllerConnection()
    {
        if (!_prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    _playerIndex = testPlayerIndex;
                }
            }
        }
    }
    private void OnDestroy()
    {
        GamePad.SetVibration(_playerIndex, 0, 0);
    }
}
