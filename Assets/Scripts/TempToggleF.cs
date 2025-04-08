using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TempToggleF : MonoBehaviour
{
    [SerializeField] private GameObject FPanel;
    private bool isTriggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PressFText());
            isTriggered = true;
        }
    }

    private IEnumerator PressFText()
    {
        yield return new WaitForSeconds(2f);
        FPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        FPanel.SetActive(false);
    }
}
