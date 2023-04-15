using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SprintAbility : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float drainRate = 1;
    [SerializeField] private float recoveryRate = 2;
    [SerializeField] private float recoveryDelayInSeconds = 2;
    [SerializeField] private float recoveryDelayInSecondsOnDepletion = 4;
    [SerializeField] private float staminaThreshold = 30;
    [SerializeField] private Slider staminaBarSlider;

    private float _currentStamina;
    private StaminaState _staminaState;
    private bool _reachedThreshold;

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnStaminaDepleted;
    public event EventHandler OnStaminaReachedThreshold;

    private void Awake()
    {
        _playerInputActions = new();
        _currentStamina = maxStamina;

        staminaBarSlider.minValue = 0;
        staminaBarSlider.maxValue = maxStamina;
    }

    private void Start()
    {
        _staminaState = StaminaState.Idle;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Sprint.performed += HandleSprintPerformed;
        _playerInputActions.Player.Sprint.canceled += HandleSprintCancelled;

        _playerInputActions.Player.Sprint.Enable();
    }


    private void OnDisable()
    {
        _playerInputActions.Player.Sprint.Disable();
    }

    private void HandleSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_currentStamina > 0 && _staminaState != StaminaState.Depleted)
        {
            _staminaState = StaminaState.Draining;
        }
    }
    private void HandleSprintCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_staminaState == StaminaState.Draining || _staminaState == StaminaState.Idle)
        {
            _staminaState = StaminaState.Idle;
            StartStaminaRecovery(recoveryDelayInSeconds);
        }
    }

    private void StartStaminaRecovery(float delay)
    {
        StartCoroutine(StaminaRecoveryDelay(delay));
    }

    private void FixedUpdate()
    {
        DoStaminaState();
        UpdateSliderBar();
    }

    private void UpdateSliderBar()
    {
        staminaBarSlider.value = _currentStamina;
    }

    private void StaminaDepleted()
    {
        _currentStamina = 0;
        _reachedThreshold = false;
        _staminaState = StaminaState.Depleted;
        StartStaminaRecovery(recoveryDelayInSecondsOnDepletion);
        OnStaminaDepleted?.Invoke(this, EventArgs.Empty);
       
    }

    private void DoStaminaState()
    {
        switch (_staminaState)
        {
            case StaminaState.Idle:
                break;
            case StaminaState.Draining:
                DrainStamina();
                break;
            case StaminaState.Depleted:
                break;
            case StaminaState.Recovering:
                RecoverStamina();
                break;
        }
    }

    private void DrainStamina()
    {
        _currentStamina -= drainRate;
        Debug.Log("Stamina: " + _currentStamina);
        if (_currentStamina <= 0)
        {
            StaminaDepleted();
            return;
        }
    }

    private void RecoverStamina()
    {
        if (_currentStamina > staminaThreshold && _reachedThreshold == false)
        {
            OnStaminaReachedThreshold?.Invoke(this, EventArgs.Empty);
            _reachedThreshold = true;
        }
        if (_currentStamina >= maxStamina)
        {
            _currentStamina = 100;
            _staminaState = StaminaState.Idle;
            return;
        }
        _currentStamina += recoveryRate;
        Debug.Log("Stamina: " + _currentStamina);
    }

    private IEnumerator StaminaRecoveryDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        _staminaState = StaminaState.Recovering;
    }

    private enum StaminaState
    {
        Idle,
        Draining,
        Recovering,
        Depleted,
    }
}
