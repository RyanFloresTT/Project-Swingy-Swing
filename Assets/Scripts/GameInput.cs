using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    private static GameInput _instance = null;
    public static GameInput Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        CheckInstance();
        _playerInputActions = new PlayerInputActions();
    }

    private void CheckInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }
    
    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return _playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
    }

    public float GetMouseXDelta()
    {
        return _playerInputActions.Player.MouseLookX.ReadValue<float>();
    }
    public float GetMouseYDelta()
    {
        return _playerInputActions.Player.MouseLookY.ReadValue<float>();
    }
}
