using System;
using UnityEngine;

public class GrappleRange : MonoBehaviour
{
    public bool IsPlayerInRange { get; private set; } = false;
    public event EventHandler OnRangeToggle;

    private void OnTriggerEnter(Collider other)
    {
        TogglePlayerInRange(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TogglePlayerInRange(other);
    }

    private void TogglePlayerInRange(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;
        IsPlayerInRange = !IsPlayerInRange;
        OnRangeToggle?.Invoke(this, EventArgs.Empty);
    }

}
