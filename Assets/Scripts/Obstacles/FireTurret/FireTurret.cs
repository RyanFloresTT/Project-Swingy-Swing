using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : MonoBehaviour
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float shotIntervalInSeconds;
    [SerializeField] private float shotSpeed;
    [SerializeField] private float fireballDeathInSeconds;

    private void Start()
    {
        StartCoroutine(ShootFireballOnInterval());
    }

    private IEnumerator ShootFireballOnInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(shotIntervalInSeconds);
            var shotDirectionWithSpeed = spawnPoint.forward * shotSpeed;
            var shotFireBall = Instantiate(fireball);
            shotFireBall.transform.position = spawnPoint.position;
            shotFireBall.GetComponent<Rigidbody>().AddForce(shotDirectionWithSpeed, ForceMode.Impulse);
            Destroy(shotFireBall, fireballDeathInSeconds);
        }
    }
}
