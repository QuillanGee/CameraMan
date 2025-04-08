using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private List<int> lineIndices;
    private bool hasTriggered = false; // Flag to track if the sound has already played

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            AudioManager.instance.PlayDialogue(lineIndices);
            hasTriggered = true;
        }
    }
}
