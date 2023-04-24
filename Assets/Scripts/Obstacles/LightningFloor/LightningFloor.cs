using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LightningFloor : MonoBehaviour
{
    [SerializeField] private GameObject lightningAnimationPrefab;
    [SerializeField] private Transform trapPosition;
    [SerializeField] private int damage;
    [SerializeField] private float tickRateInSeconds;
    [SerializeField] private float stunDuration;
    [SerializeField] [Range(0.0f, 1.0f)] private float slowAmountInPercentage;

    private void Start()
    {
        lightningAnimationPrefab.gameObject.transform.position = trapPosition.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Player>(out var player)) return;
        SlowPlayer(player);
        StartCoroutine(ApplyDamage(player));
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Player>(out var player)) return;
        NormalizePlayerSpeed(player);
        StopAllCoroutines();
    }

    private IEnumerator ApplyDamage(Player player)
    {
        while (true)
        {
            PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
            StartCoroutine(StunPlayer(playerMovement));
            player.TakeDamage(damage);
            yield return new WaitForSeconds(tickRateInSeconds);
        }
    }

    private IEnumerator StunPlayer(PlayerMovement playerMovement)
    {
        playerMovement.DisableMovement();
        yield return new WaitForSeconds(stunDuration);
        playerMovement.EnableMovement();
    }

    private void SlowPlayer(Player player)
    {
        PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerMovement.ChangePlayerSpeedMultiplier(slowAmountInPercentage);
    }

    private void NormalizePlayerSpeed(Player player)
    {
        PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerMovement.ChangePlayerSpeedMultiplier(1f);
    }
}
