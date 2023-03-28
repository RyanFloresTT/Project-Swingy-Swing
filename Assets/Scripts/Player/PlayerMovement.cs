using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private Transform sphereCheckTransform;
    [SerializeField] private float sphereRadius;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Transform playerForwardOrientation;
    [SerializeField] private float playerRotationSpeed;
    
    private bool _isJumping;
    private PlayerInputActions _playerInputActions;
    private GameInput _gameInput;
    private Rigidbody _rb;
    private bool _isMoving;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Jump.performed += HandleJumpPerformed;
        _playerInputActions.Player.Jump.Enable();
    }


    private void OnDisable()
    {
        _playerInputActions.Player.Jump.Disable();
    }

    private void HandleJumpPerformed(InputAction.CallbackContext obj)
    {
        Jump();
    }

    private void Start()
    {
        _isMoving = false;
        _gameInput = GameInput.Instance;
    }

    private void Update()
    {
        Move();
        ApplyDownwardForce();
    }

    private void ApplyDownwardForce()
    {
        if (_isJumping)
        {
            _rb.AddForce(Vector3.down * fallMultiplier);
        }
    }

    private void Jump()
    {
        _isJumping = true;
        float jumpForce = Mathf.Sqrt(maxJumpHeight * -2 * (Physics.gravity.y));
        _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }

    private void Move()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 inputDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if (inputDirection != Vector3.zero)
        {
            _rb.AddForce(inputDirection * moveSpeed);
            _isMoving = true;
        } else
        {
            _isMoving= false;
        }
    }
}
