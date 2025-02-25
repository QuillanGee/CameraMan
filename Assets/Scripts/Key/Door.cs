using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isUnlocked = false;
    [SerializeField] private GameObject door2D;

    private void Start()
    {
        EventManager.instance.OnUnlockDoor += OpenDoor;
    }
    
    public void UnlockDoor()
    {
        isUnlocked = true;
        Debug.Log(gameObject.name + " is now unlocked!");
    }

    private void OpenDoor()
    {
        Destroy(gameObject);
        Destroy(door2D);
    }
}
