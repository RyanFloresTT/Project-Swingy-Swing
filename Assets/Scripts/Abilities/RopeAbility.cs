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
    private LineRenderer _playerRope;
    private Vector3 _ropeEnd;
    private float _ropeWidth = 0.03f;

    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grappleRange;
    [SerializeField] private bool isDebugOn;
    [SerializeField] private Material ropeMaterial;
    [SerializeField] private Transform ropeStart;

    [SerializeField] private float springForce = 20f;
    [SerializeField] private float damperForce = 0f;

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
        Destroy(player.GetComponent<LineRenderer>());
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
            UpdateSpringJoint(ropeBody);
            UpdateLineRenderer();
        } else
        {
            Debug.Log("More than 1 spring joint on playerl.");
        }
    }


private void Update()
    {
        DrawDebugRays();
        UpdateRopeEnds();
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

    private void UpdateRopeEnds()
    {
        if (_playerRope == null) return;
        _playerRope.SetPosition(0, ropeStart.position);
        _playerRope.SetPosition(1, _ropeEnd);
    }

    private void HandleGrapplePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        if (!Physics.Raycast(_mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).origin, _mainCamera.transform.forward, out hit, grappleRange, grappleLayer)) return;
        Debug.Log("Grapple Point Found.");
        _ropeEnd = hit.transform.position;
        RopeTo(hit);
    }
    
    private void UpdateSpringJoint(Rigidbody ropeBody)
    {
        _springJoint = player.AddComponent<SpringJoint>();
        _springJoint.connectedBody = ropeBody;
        _springJoint.spring = springForce;
        _springJoint.damper = damperForce;
        _springJoint.autoConfigureConnectedAnchor = false;
    }

    private void UpdateLineRenderer()
    {
        _playerRope = player.AddComponent<LineRenderer>();
        _playerRope.material = ropeMaterial;
        _playerRope.widthMultiplier = _ropeWidth;
    }
}
