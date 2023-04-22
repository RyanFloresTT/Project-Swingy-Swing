using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLevelButton : MonoBehaviour
{
    private int _currentLevelIndex;
    private void Start()
    {
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(_currentLevelIndex);
    }
}
