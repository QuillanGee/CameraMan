using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveConnector : MonoBehaviour
{
    private bool isOnPlatform;
    [SerializeField] private Transform LinkedObject; 
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnToggleFirstPerson += CheckIsOnPlatform;
    }

    private void CheckIsOnPlatform()
    {
        if (isOnPlatform)
        {
            EventManager.instance.SendZAxis(LinkedObject.position.z, isOnPlatform);
        }
        else
        {
            EventManager.instance.SendZAxis(-1f, isOnPlatform);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isOnPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isOnPlatform = false;
    }
}
