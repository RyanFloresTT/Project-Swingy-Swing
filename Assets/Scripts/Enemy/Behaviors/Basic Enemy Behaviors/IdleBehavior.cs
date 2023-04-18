using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : MonoBehaviour, IHaveIdleBehaviors
{
    public void Tick()
    {
        Debug.Log("Idle Tick");
    }

    public void FixedTick()
    {
    }
}
