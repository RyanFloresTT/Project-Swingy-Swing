using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour, ITakeDamage
{
    public static Player Instance;

    public event EventHandler OnPlayerDeath;

    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthSlider;

    private int _currentHealth;

    private void Awake()
    {
        CheckInstance();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealth();
    }

    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int health)
    {
        _currentHealth += health;
        UpdateHealth();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            KillPlayer();
        }
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = _currentHealth;
    }

    private void KillPlayer()
    {
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }
}
