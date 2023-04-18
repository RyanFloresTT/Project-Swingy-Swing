using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHandler : MonoBehaviour
{
    [SerializeField] private EnemyState _currentState;

    [SerializeField] private IHaveIdleBehaviors _idleBehavior;
    [SerializeField] private IHaveChaseBehaviors _chaseBehaviors;
    [SerializeField] private IHaveAttackBehaviors _attackBehaviors;
    [SerializeField] private IHaveDeathBehaviors _deathBehaviors;

    private void Start()
    {
        _currentState = EnemyState.Idle;
    }

    private void Update()
    {
        DoStateLogic();
    }

    private void DoStateLogic()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                _idleBehavior.Tick();
                break;
            case EnemyState.Chase:
                _chaseBehaviors.Tick();
                break;
            case EnemyState.Attack:
                _attackBehaviors.Tick();
                break;
            case EnemyState.Death:
                _deathBehaviors.Tick();
                break;
        }
    }

    public void ChangeState(EnemyState state)
    {
        _currentState = state;
    }
}
