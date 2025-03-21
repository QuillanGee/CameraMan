using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private bool isHeld = false;
    private Collider collider;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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

        isHeld = true;
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;
        transform.SetParent(holdPosition);
        rb.isKinematic = true;
        collider.enabled = false;
    }

    public void Drop()
    {
        if (!isHeld) return;
        isHeld = false;
        rb.isKinematic = false;
        transform.parent = null;
        collider.enabled = true;
    }
}