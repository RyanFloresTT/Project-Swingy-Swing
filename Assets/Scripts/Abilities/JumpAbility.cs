using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class JumpAbility : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float cooldownTimeInSeconds = 4f;
    [SerializeField] private Image cooldownImage;

    private bool _isReady;
    private float _currentCooldownTime;
    private PlayerInputActions _playerInputActions;
    private Rigidbody _playerRigidBody;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _isReady = true;
        _currentCooldownTime = 0;
        cooldownImage.fillAmount = 1;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.JumpAbiltiy.performed += Handle_JumpAbility_Performed;
        _playerInputActions.Player.JumpAbiltiy.Enable();
    }

    private void Handle_JumpAbility_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!_isReady) return;
        PerformJumpAbility();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.JumpAbiltiy.Disable();
    }

    private void PerformJumpAbility()
    {
        _isReady = false;
        Vector3 direction = Vector3.up;
        _playerRigidBody.AddForce(direction * jumpForce, ForceMode.Impulse);

    }

    private void FixedUpdate()
    {
        if (!_isReady)
        {
            _currentCooldownTime += Time.deltaTime;
            cooldownImage.fillAmount = _currentCooldownTime / cooldownTimeInSeconds;
            if (_currentCooldownTime >= cooldownTimeInSeconds)
            {
                _currentCooldownTime = 0;
                _isReady = true;
            }
        }
    }

}
