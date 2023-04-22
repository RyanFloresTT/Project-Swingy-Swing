using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = Player.Instance;
        gameObject.SetActive(false);
        _player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void HandlePlayerDeath(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
