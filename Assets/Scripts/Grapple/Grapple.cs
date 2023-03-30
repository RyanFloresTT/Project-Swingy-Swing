using System;
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
    private Camera _mainCamera;
          
    [SerializeField] private GameObject player;                   
    [SerializeField] private Transform grappleStartPoint;        
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grappleRange;
    [SerializeField] private bool isDebugOn;

    private void Awake()
    {
        _grappleRenderer = GetComponent<LineRenderer>();
        _playerInputActions = new PlayerInputActions();
        _mainCamera = Camera.main;
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

    private void Update()
    {
        DrawDebugRays();
    }

    private void DrawDebugRays()
    {
        if (isDebugOn)
        {
            if (Physics.Raycast(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).origin, _mainCamera.transform.forward, grappleRange, grappleLayer))
            {
                Debug.DrawRay(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).origin, _mainCamera.transform.forward * grappleRange, Color.green);
            }
            else
            {
                Debug.DrawRay(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).origin, _mainCamera.transform.forward * grappleRange, Color.red);
            }
        }
    }

    private void HandleGrapplePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).origin, _mainCamera.transform.forward, grappleRange, grappleLayer))
        {
            Debug.Log("Grapple Hit!");
        }
        else
        {
            Debug.Log("Grapple Hit NOTHING!");
        }
    }

    private void HandleGrappleCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
    }
}
