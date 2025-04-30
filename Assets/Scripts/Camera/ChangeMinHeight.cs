using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMinHeight : MonoBehaviour
{
    [SerializeField] private CameraFollow camFollow;
    [SerializeField] private Transform newMinHeight;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player2D"))
        {
            camFollow.SetMinBottom(newMinHeight);
        }
    }
}
