using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public Material defaultMaterial;
    public Material hoverMaterial;
    private Renderer objectRenderer;

    private Rigidbody rb;
    private Collider collider;
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        if (objectRenderer != null)
        {
            
        }
    }

    public void OnHoverEnter()
    {
        if (objectRenderer != null)
        {
            print("Hover");
        }
    }

    public void OnHoverExit()
    {
        if (objectRenderer != null)
        {
            print("Left Hover");
        }
    }
    
    public void Pickup(Transform holdPosition)
    {
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;
        transform.SetParent(holdPosition);
        rb.isKinematic = true;
        collider.enabled = false;
    }

    public void Drop()
    {
        rb.isKinematic = false;
        transform.parent = null;
        collider.enabled = true;
    }

    public void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}