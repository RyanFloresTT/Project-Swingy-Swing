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
    [SerializeField] private LayerMask layerMask; 
    
    private bool _isJumping;
    private bool _isJumpCancelled;
    private float _jumpTime;
    private PlayerInputActions _playerInputActions;
    private Rigidbody _rb;
    private bool _isWalking;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Jump.performed += Jump_OnPerformed;
        _playerInputActions.Player.Jump.canceled += Jump_OnCancelled;
        _playerInputActions.Player.Jump.Enable();
    }


    private void OnDisable()
    {
        _playerInputActions.Player.Jump.Disable();
    }

    private void Jump_OnPerformed(InputAction.CallbackContext obj)
    {
        Jump();
    }
        
    private void Jump_OnCancelled(InputAction.CallbackContext obj)
    {
        _isJumpCancelled = true;
    }

    private void Update()
    {
        Move();
        AddJumpTime();

        if (!Physics.CheckSphere(sphereCheckTransform.position, sphereRadius, layerMask)) return;
        Debug.Log("Hit Floor");
        _isJumping = false;
        _isJumpCancelled = false;
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

    private void FixedUpdate()
    {
        if (_isJumpCancelled && _isJumping)
        {
            _rb.AddForce(Vector3.down * fallMultiplier);
        }
    }

    private void Move()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (inputVector.x != 0 || inputVector.y != 0)
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
        _rb.AddForce(moveDir * moveSpeed);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}
