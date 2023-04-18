using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(EnemyStateHandler))]
public class EnemyStateChanger : MonoBehaviour
{
    private EnemyStateHandler _stateHandler;

    public event EventHandler OnEnemyStateChanged;

    private void Awake()
    {
        _stateHandler = GetComponent<EnemyStateHandler>();
    }

    private void Update()
    {
        CheckStateRequirements();
    }

    private void CheckStateRequirements()
    {
    }

}
