using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SprintAbility), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Transform playerForwardOrientation;
    [SerializeField] private float playerRotationSpeed;
    [SerializeField] private float isGroundedHeight;
    [SerializeField] private Transform feetSpace;

    private float _moveSpeed;
    private PlayerInputActions _playerInputActions;
    private GameInput _gameInput;
    private Rigidbody _rb;
    private bool _isMoving;
    private bool _canSprint = true;
    private bool _isGrounded = true;
    private SprintAbility _sprintAbility;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();
        _sprintAbility = GetComponent<SprintAbility>();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Jump.performed += HandleJumpPerformed;

        _playerInputActions.Player.Sprint.performed += HandleSprintPerformed;
        _playerInputActions.Player.Sprint.canceled += HandleSprintCancelled;

        _sprintAbility.OnStaminaDepleted += HandleStaminaDepleted;
        _sprintAbility.OnStaminaReachedThreshold += HandleStaminaReachedThreshold;

        _playerInputActions.Player.Jump.Enable();
        _playerInputActions.Player.Sprint.Enable();
    }

    private void HandleStaminaReachedThreshold(object sender, EventArgs e)
    {
        _canSprint = true;
    }

    private void HandleStaminaDepleted(object sender, EventArgs e)
    {
        _canSprint = false;
        _moveSpeed = walkSpeed;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Jump.Disable();
        _playerInputActions.Player.Sprint.Disable();
    }
    private void Start()
    {
        _isMoving = false;
        _gameInput = GameInput.Instance;
        _moveSpeed = walkSpeed;
    }

    private void Update()
    {
        Move();
        ApplyDownwardForce();
        DoRayCast();
    }

    private void HandleSprintCancelled(InputAction.CallbackContext obj)
    {
        _moveSpeed = walkSpeed;
    }

    private void HandleSprintPerformed(InputAction.CallbackContext obj)
    {
        if (_canSprint)
        {
            _moveSpeed = runSpeed;
        }
    }


    private void HandleJumpPerformed(InputAction.CallbackContext obj)
    {
        Jump();
    }

    private void ApplyDownwardForce()
    {
        if (!_isGrounded)
        {
            _rb.AddForce(Vector3.down * fallMultiplier);
        }
    }

    private void DoRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(feetSpace.position, transform.TransformDirection(Vector3.down), out hit, isGroundedHeight))
        {
            _isGrounded = true;
        } else
        {
            _isGrounded = false;
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            //_rb.velocity = Vector3.zero;
            float jumpForce = Mathf.Sqrt(maxJumpHeight * -2 * (Physics.gravity.y));
            _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    private void Move()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 inputDirection = playerForwardOrientation.forward * inputVector.y + playerForwardOrientation.right * inputVector.x;

        if (inputVector != Vector2.zero)
        {
            _rb.AddForce(inputDirection * _moveSpeed);
            _isMoving = true;
        } else
        {
            _isMoving= false;
        }
    }

}
