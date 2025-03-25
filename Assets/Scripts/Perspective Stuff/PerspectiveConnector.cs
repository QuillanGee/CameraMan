using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveConnector : MonoBehaviour
{
    [SerializeField] private Transform LinkedObject; 
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void SendIsOnPlatform()
    {
        EventManager.instance.SendZAxis(LinkedObject.position.z);
        print("Was on platform");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EventManager.instance.OnToggleFirstPerson += SendIsOnPlatform;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        EventManager.instance.OnToggleFirstPerson -= SendIsOnPlatform;
    }

    private IEnumerator PlatformBuffer()
    {
        yield return new WaitForSeconds(1f);
    }
}
