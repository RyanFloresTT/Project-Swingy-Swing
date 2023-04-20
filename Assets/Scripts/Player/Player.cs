using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void Heal(int health)
    {
        _currentHealth += health;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }
}
