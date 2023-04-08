using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private GrappleRange grappleRange;
    [SerializeField] private GameObject grappleReadyIcon;

    private Material grappleMaterial;

    private void Start()
    {
        grappleMaterial = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        grappleRange.OnRangeToggle += HandleRangeToggle;
    }

    private void HandleRangeToggle(object sender, EventArgs e)
    {
        Debug.Log(grappleRange.IsPlayerInRange);
        ToggelMeshColor();
    }

    private void ToggelMeshColor()
    {
        if (grappleRange.IsPlayerInRange)
        {
            grappleMaterial.color = Color.green;
        }
        else
        {
            grappleMaterial.color = Color.red;
        }
    }
}
