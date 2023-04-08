using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RopeAbility : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Camera _mainCamera;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform grappleStartPoint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grappleRange;
    [SerializeField] private bool isDebugOn;
    [SerializeField] private Transform grappleEndPoint;
    [SerializeField] private float grappleForce;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.RopeAbility.performed += HandleGrapplePerformed;
        _playerInputActions.Player.RopeAbility.canceled += HandleGrappleCancelled;
        _playerInputActions.Player.RopeAbility.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.RopeAbility.Disable();
    }

    private void RopeTo(Vector3 grapplePoint)
    {
        Vector3 direction = (grapplePoint - player.transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(direction * grappleForce, ForceMode.Impulse);
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
        if (!Physics.Raycast(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).origin, _mainCamera.transform.forward, out hit, grappleRange, grappleLayer)) return;
        Debug.Log("Grapple Point Found.");
        RopeTo(hit.point);
    }

    private void HandleGrappleCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
    }
}
