using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform headSpaceCameraPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] float rotationSpeed;
    private GameInput _gameInput;

    private void Start()
    {
        _gameInput = GameInput.Instance;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        RotatePlayer();
        RotateCamera();
    }

    private void RotateCamera()
    {
        var mouseXRotation = _gameInput.GetMouseYDelta();
        cameraTransform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed * mouseXRotation);
    }

    private void RotatePlayer()
    {
        var mouseXRotation = _gameInput.GetMouseXDelta();
        playerTransform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * mouseXRotation);
    }
}