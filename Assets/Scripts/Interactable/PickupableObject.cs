using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class PickableObject : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private bool isHeld = false;
    private List<Collider> colliders = new List<Collider>();  // Declare a List for colliders
    public string hoverText = "(Right Click)";
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get all colliders on the parent GameObject and add them to the list
        colliders.AddRange(GetComponents<Collider>());

        // Loop through all colliders in children and descendants
        Collider[] allChildColliders = GetComponentsInChildren<Collider>(); // This gets all colliders in children and descendants

        foreach (Collider childCollider in allChildColliders)
        {
            // Check if the child collider's GameObject is tagged as "PickUp"
            if (childCollider.CompareTag("PickUp") || childCollider.CompareTag("Door") || childCollider.CompareTag("whiteboard"))
            {
                // Add the collider of the child object to the list
                colliders.Add(childCollider);
            }
        }
    }

    
    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }

    public void OnInteract()
    {
        // Interact behavior (could be overridden in subclasses)
    }

    public void Pickup(Transform holdPosition)
    {
        if (isHeld) return;
        AudioManager.instance.PlayInteractionSound(gameObject.tag);
        isHeld = true;
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;
        transform.SetParent(holdPosition);
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.None;
        
        // Disable all colliders
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void Drop()
    {
        if (!isHeld) return;
        isHeld = false;
        rb.isKinematic = false;
        transform.parent = null;
        
        // Re-enable all colliders
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public string GetText()
    {
        return hoverText;
    }
}
