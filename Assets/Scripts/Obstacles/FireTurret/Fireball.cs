using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnCollisionEnter(Collision collision)
    {
        var targetCollider = collision.gameObject.GetComponent<ITakeDamage>();
        if (targetCollider != null)
        {
            targetCollider.TakeDamage(damage);
        }
    }
}
