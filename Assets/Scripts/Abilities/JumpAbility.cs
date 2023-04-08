using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private PlayerInputActions _playerInputActions;
    private Rigidbody _playerRigidBody;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.JumpAbiltiy.performed += Handle_JumpAbility_Performed; ;
        _playerInputActions.Player.JumpAbiltiy.canceled += Handle_JumpAbility_Cancelled;
        _playerInputActions.Player.JumpAbiltiy.Enable();
    }

    private void Handle_JumpAbility_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PerformJumpAbility();
    }

    private void Handle_JumpAbility_Cancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
    }


    private void OnDisable()
    {
        _playerInputActions.Player.JumpAbiltiy.Disable();
    }

    private void PerformJumpAbility()   
    {
        Vector3 direction = Vector3.up;
        _playerRigidBody.AddForce(direction * jumpForce, ForceMode.Impulse);
    }

}
