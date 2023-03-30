using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Transform playerForwardOrientation;
    [SerializeField] private float playerRotationSpeed;
    [SerializeField] private float isGroundedHeight;
    [SerializeField] private Transform feetSpace;
    
    private PlayerInputActions _playerInputActions;
    private GameInput _gameInput;
    private Rigidbody _rb;
    private bool _isMoving;
    private bool _isGrounded = true;

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
        DoRayCast();
        Debug.Log("IsGrounded: " + _isGrounded);
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
            _rb.velocity = Vector3.zero;
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
            _rb.AddForce(inputDirection * moveSpeed);
            _isMoving = true;
        } else
        {
            _isMoving= false;
        }
    }
}
