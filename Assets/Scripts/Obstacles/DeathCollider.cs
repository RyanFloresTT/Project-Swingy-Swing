using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ITakeDamage entity = other.GetComponent<ITakeDamage>();
        if (entity == null) return;
        entity.TakeDamage(100000);
    }
}
