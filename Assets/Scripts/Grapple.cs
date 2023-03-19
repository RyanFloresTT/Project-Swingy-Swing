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

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform player;
    [SerializeField] private Transform grappleStartPoint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float springElasticity = 4.5f;
    [SerializeField] private float springDamper = 7f;
    [SerializeField] private float springMassScale = 4.5f;

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
        RaycastHit grappleHit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out grappleHit, _maxGrappleDistance, grappleLayer))
        {
            _grapplePoint = grappleHit.point;
            _playerToGrappleSpring = player.gameObject.AddComponent<SpringJoint>();
            _playerToGrappleSpring.autoConfigureConnectedAnchor= false;
            _playerToGrappleSpring.connectedAnchor = _grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, _grapplePoint);

            _playerToGrappleSpring.maxDistance = distanceFromPoint;
            _playerToGrappleSpring.minDistance = distanceFromPoint;

            _playerToGrappleSpring.spring = springElasticity;
            _playerToGrappleSpring.damper = springDamper;
            _playerToGrappleSpring.massScale = springMassScale;

        }
    }
    private void HandleGrappleCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
    }
}
