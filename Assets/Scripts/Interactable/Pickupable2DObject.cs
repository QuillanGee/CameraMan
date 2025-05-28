using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class Pickable2DObject : MonoBehaviour, IInteractable
{
    // private Rigidbody2D rb;
    private bool isHeld = false;
    private List<Collider2D> colliders = new List<Collider2D>();  // Declare a List for colliders
    public string hoverText = "(Right Click)";
    
    private void Start()
    {
        // rb = GetComponent<Rigidbody2D>();

        // Get all colliders on the parent GameObject and add them to the list
        colliders.AddRange(GetComponents<Collider2D>());

        // Loop through all colliders in children and descendants
        Collider2D[] allChildColliders = GetComponentsInChildren<Collider2D>(); // This gets all colliders in children and descendants

        foreach (Collider2D childCollider in allChildColliders)
        {
            // Check if the child collider's GameObject is tagged as "PickUp"
            if (childCollider.CompareTag("PickUp") || childCollider.CompareTag("Door"))
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
        // rb.isKinematic = true;
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;
        transform.SetParent(holdPosition);
        
        // Disable all colliders
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void Drop()
    {
        print("Dropped");
        if (!isHeld) return;
        isHeld = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        // rb.isKinematic = false;
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
