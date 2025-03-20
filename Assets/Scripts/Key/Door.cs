using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isUnlocked = false;
    // [SerializeField] private GameObject door2D;
    private Animator doorAnimator;
    [SerializeField] private string animationBoolName;
    [SerializeField] Collider doorCollider;  // Store reference to the collider


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
            UnlockDoor();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LockDoor();
        }
    }
    
    public void UnlockDoor()
    {
        isUnlocked = true;
        doorAnimator.SetBool(animationBoolName, true);
        // StartCoroutine(DisableCollisionTemporarily()); // Disable collision while opening
    }
    
    private void LockDoor()
    {
        
        isUnlocked = true;
        doorAnimator.SetBool(animationBoolName, false);
    }
    
    IEnumerator DisableCollisionTemporarily()
    {
        doorCollider.enabled = false; // Disable collision
        yield return new WaitForSeconds(doorAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for animation to finish
        doorCollider.enabled = true;  // Re-enable collision
    }

    // private void OpenDoor()
    // {
    //     Destroy(gameObject);
    //     Destroy(door2D);
    // }
}