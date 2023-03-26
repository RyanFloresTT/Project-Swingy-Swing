using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grapple : MonoBehaviour
{

    private LineRenderer _grappleRenderer;
    private Vector3 _grapplePoint;                               
    private PlayerInputActions _playerInputActions;              
    private float _maxGrappleDistance = 100f;                    
    private SpringJoint _playerToGrappleSpring;                  
          
    [SerializeField] private GameObject player;                   
    [SerializeField] private Transform grappleStartPoint;        
    [SerializeField] private LayerMask grappleLayer;       

    private void Awake()
    {
        _grappleRenderer = GetComponent<LineRenderer>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Grapple.performed += HandleGrapplePerformed;
        _playerInputActions.Player.Grapple.canceled += HandleGrappleCancelled;
        _playerInputActions.Player.Grapple.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Grapple.Disable();
    }

    private void HandleGrapplePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
    }
    private void HandleGrappleCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
    }
}
