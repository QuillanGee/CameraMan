using TMPro;
using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private bool isHeld = false;
    private Collider[] colliders;
    public string hoverText = "(Right Click)";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>(); // Get all colliders on the object
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
