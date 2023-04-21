using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] stoppingPoints;
    [SerializeField] private Transform platform;
    [SerializeField] private float waitTimeInSeconds;
    [SerializeField] private float speed;

    private int _currentPoint;
    private int _nextPoint;
    private bool _canMove = true;

    private void Start()
    {
        _currentPoint = 0;
        _nextPoint = 1;
        platform.position = stoppingPoints[_currentPoint].position;
    }

    private void Update()
    {
        MovePlatform();
        CheckPlatformReachedDestination();
    }

    private void GetNextPoints()
    {
        _currentPoint = GetNextIndex(_currentPoint);
        _nextPoint = GetNextIndex(_nextPoint);
        Debug.Log("Current Point: " + _currentPoint + ". Next Point: " + _nextPoint);
    }

    private void MovePlatform()
    {
        if (_canMove)
        {
            var step = speed * Time.deltaTime;
            platform.position = Vector3.MoveTowards(platform.position, stoppingPoints[_nextPoint].position, step);
        }
    }

    private int GetNextIndex(int currentIndex)
    {
        if (CheckIndexOutOfBounds(currentIndex))
        {
            return 0;
        } else
        {
            return currentIndex + 1;
        }
    }

    private bool CheckIndexOutOfBounds(int index)
    {
        if (index + 1 > stoppingPoints.Length - 1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void CheckPlatformReachedDestination()
    {
        if (!_canMove) return;
        if (!(platform.position == stoppingPoints[_nextPoint].position)) return;
        StartCoroutine(GoToNextPoints());
    }

    private IEnumerator GoToNextPoints()
    {
        _canMove = false;
        yield return new WaitForSeconds(waitTimeInSeconds);
        GetNextPoints();
        _canMove = true;
        Debug.Log("Reached Point");
    }
}
