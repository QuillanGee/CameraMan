using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isUnlocked = false;
    [SerializeField] private GameObject door2D;
    private Animator doorAnimator;

    private void Start()
    {
        EventManager.instance.OnUnlockDoor += UnlockDoor;
        EventManager.instance.OnCloseDoor += LockDoor;
        doorAnimator = GetComponent<Animator>();
    }
    
    public void UnlockDoor()
    {
        isUnlocked = true;
        doorAnimator.SetBool("OpenRight", true);
    }
    
    private void LockDoor()
    {
        isUnlocked = true;
        doorAnimator.SetBool("OpenRight", false);
    }

    private void OpenDoor()
    {
        Destroy(gameObject);
        Destroy(door2D);
    }
}
