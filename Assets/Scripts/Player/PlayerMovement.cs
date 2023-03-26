using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float maxJumpHoldDuration;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private Transform sphereCheckTransform;
    [SerializeField] private float sphereRadius;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Transform playerForwardOrientation;
    [SerializeField] private float playerRotationSpeed;
    
    private bool _isJumping;
    private bool _isJumpCancelled;
    private float _jumpTime;
    private PlayerInputActions _playerInputActions;
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
        _playerInputActions.Player.Jump.canceled += HandleJumpCancelled;
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
        
    private void HandleJumpCancelled(InputAction.CallbackContext obj)
    {
        _isJumpCancelled = true;
    }

    private void Start()
    {
        _isMoving = false;
    }

    private void Update()
    {
        Move();
        AddJumpTime();
        ApplyDownwardForce();
        CheckIfGrounded();
    }

    private void CheckIfGrounded()
    {
        // TODO : Make this into a Raycast
        if (!Physics.CheckSphere(sphereCheckTransform.position, sphereRadius, floorLayer)) return;
        _isJumping = false;
        _isJumpCancelled = false;
    }

    private void ApplyDownwardForce()
    {
        if (_isJumpCancelled && _isJumping)
        {
            _rb.AddForce(Vector3.down * fallMultiplier);
        }
    }

    private void AddJumpTime()
    {
        if (!_isJumping) return;
        _jumpTime += Time.deltaTime;
        
        if (_jumpTime > maxJumpHoldDuration)
        {
            _isJumping = false;
        }
    }

    private void Jump()
    {
        _jumpTime = 0;
        _isJumping = true;
        _isJumpCancelled = false;
        float jumpForce = Mathf.Sqrt(maxJumpHeight * -2 * (Physics.gravity.y));
        _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }

    private void Move()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 inputDirection = playerForwardOrientation.forward * inputVector.y + playerForwardOrientation.right* inputVector.x;   

        if (inputVector.y != 0)
        {
            _rb.AddForce(inputDirection * moveSpeed);
            _isMoving = true;
        } else
        {
            _isMoving= false;
        }
        if (inputVector.x != 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, inputDirection.normalized, Time.deltaTime * playerRotationSpeed);
        }
    }
}
