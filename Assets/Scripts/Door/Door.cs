using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool openLeft = false;
    [SerializeField] private bool openRight = false;
    [SerializeField] private bool keepOpen = false;
    [SerializeField] private bool disableDoorCollider = false;
    [SerializeField] private bool lockDoorAfterClosing = false;
    private bool keepDoorclosed = false;

    // [SerializeField] private GameObject door2D;
    private Animator doorAnimator;
    [SerializeField] Collider rightDoorCollider;
    [SerializeField] Collider leftDoorCollider;


    private void Start()
    {
        // EventManager.instance.OnUnlockDoor += UnlockDoor;
        // EventManager.instance.OnCloseDoor += LockDoor;
        doorAnimator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keepDoorclosed == true)
                return;
            if (openRight)
            {
                OpenRightDoor();
            }

            if (openLeft)
            {
                OpenLeftDoor();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!keepOpen)
            {
                CloseDoor();
                if (lockDoorAfterClosing)
                {
                    keepDoorclosed = true;
                }
            }
        }
    }
    
    private void OpenRightDoor()
    {
        doorAnimator.SetBool("OpenRight", true);
        if (disableDoorCollider)
        {
            StartCoroutine(DisableCollisionTemporarily(rightDoorCollider));
        }
    }

    public void OpenLeftDoor()
    {
        doorAnimator.SetBool("OpenLeft", true);
        if (disableDoorCollider)
        {
            StartCoroutine(DisableCollisionTemporarily(leftDoorCollider));
        }
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool("OpenRight", false);
        doorAnimator.SetBool("OpenLeft", false);
    }
    
    IEnumerator DisableCollisionTemporarily(Collider doorCollider)
    {
        doorCollider.enabled = false; // Disable collision
        yield return new WaitForSeconds(doorAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for animation to finish
        doorCollider.enabled = true;  // Re-enable collision
    }
    
}