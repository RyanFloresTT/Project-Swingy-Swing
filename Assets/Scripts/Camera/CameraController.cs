using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerHead;
    [SerializeField] private float rotationSpeed;

    private Transform _mainCameraPosition;
    private GameInput _gameInput;
    private Player _player;

    private void Start()
    {
        _gameInput = GameInput.Instance;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _player = Player.Instance;
        _player.OnPlayerDeath += HandleOnPlayerDeath;
    }

    private void HandleOnPlayerDeath(object sender, EventArgs e)
    {
        rotationSpeed = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        RotatePlayer();
        RotateCamera();;
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