using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMusic : MonoBehaviour
{
    private bool isTurnedOff = false;
    private bool isTurnedOn = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isTurnedOff && other.CompareTag("Player"))
        {
            AudioManager.instance.FadeMusicVolume(0f, 1f);
            isTurnedOff = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isTurnedOn && other.CompareTag("Player"))
        {
            AudioManager.instance.FadeMusicVolume(0.15f, 1f);
            isTurnedOn = true;
        }
    }
}
