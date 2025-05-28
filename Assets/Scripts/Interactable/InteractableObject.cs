using Cinemachine;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public Material defaultMaterial;
    public Material hoverMaterial;
    private Renderer objectRenderer;

    // private Rigidbody rb;
    // private Collider collider;
    public string hoverText = "(Right Click)";
    private CinemachineVirtualCamera interactionCamera;
    
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        // rb = GetComponent<Rigidbody>();
        // collider = GetComponent<Collider>();
        interactionCamera = GetComponentInParent<CinemachineVirtualCamera>();
        
        if (interactionCamera == null)
        {
            print("No interaction Camera Found");
        }
    }

    public void OnHoverEnter()
    {
        EventManager.instance.Hover(interactionCamera);
        // EventManager.instance.PauseGamePlay(true);
    }

    public void OnHoverExit()
    {
    }
    
    public void Pickup(Transform holdPosition)
    {
        // transform.position = holdPosition.position;
        // transform.rotation = holdPosition.rotation;
        // transform.SetParent(holdPosition);
        // rb.isKinematic = true;
        // collider.enabled = false;
    }

    public void Drop()
    {
        // rb.isKinematic = false;
        // transform.parent = null;
        // collider.enabled = true;
    }

    public void OnInteract()
    {
        AudioManager.instance.PlayInteractionSound(gameObject.tag);
        EventManager.instance.Interact();
        PerspectiveLockManager.Instance.SetLock(true);
    }

    public void OnExitInteraction()
    {
        EventManager.instance.ExitInteract();
        PerspectiveLockManager.Instance.SetLock(false);
    }

    public string GetText()
    {
        return hoverText;
    }

}