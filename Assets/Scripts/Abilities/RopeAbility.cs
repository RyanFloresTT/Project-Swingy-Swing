using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RopeAbility : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Camera _mainCamera;
    private SpringJoint _springJoint;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform grappleStartPoint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grappleRange;
    [SerializeField] private bool isDebugOn;
    [SerializeField] private Transform grappleEndPoint;
    [SerializeField] private float grappleForce;

    [SerializeField] private float springForce = 80f;
    [SerializeField] private float damperForce = 40f;

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

    private void HandleGrappleCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (player.GetComponent<SpringJoint>() == null) return;
        Destroy(player.GetComponent<SpringJoint>());
    }

    private void OnDisable()
    {
        _playerInputActions.Player.RopeAbility.Disable();
    }

    private void RopeTo(RaycastHit raycastHit)
    {
        var ropeBody = raycastHit.rigidbody;
        if (_springJoint == null)
        {
            _springJoint = player.AddComponent<SpringJoint>();
            _springJoint.connectedBody = ropeBody;
            _springJoint.spring = springForce;
            _springJoint.damper = damperForce;
            _springJoint.autoConfigureConnectedAnchor = false;
        } else
        {
            Debug.Log("More than 1 spring joint on playerl.");
        }
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
        RopeTo(hit);
    }
}
