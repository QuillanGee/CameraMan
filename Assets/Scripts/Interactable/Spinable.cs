using Cinemachine;
using UnityEngine;

public class Spinable : MonoBehaviour, IInteractable
{
    public Material defaultMaterial;
    public Material hoverMaterial;
    private Renderer objectRenderer;
    
    public string hoverText = "(Right Click)";
    private CinemachineVirtualCamera interactionCamera;
    
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        interactionCamera = GetComponentInParent<CinemachineVirtualCamera>();
        
        if (interactionCamera == null)
        {
            print("No interaction Camera Found");
        }
    }

    public void OnHoverEnter()
    {
        EventManager.instance.Hover(interactionCamera);
    }

    public void OnHoverExit()
    {
    }
    
    public void Pickup(Transform holdPosition)
    {

    }

    public void Drop()
    {
        
    }

    public void OnInteract()
    {
        AudioManager.instance.PlayInteractionSound(gameObject.tag);
        EventManager.instance.Interact();
        EventManager.instance.EnableSpinControls(transform);
        PerspectiveLockManager.Instance.SetLock(true);
    }

    public void OnExitInteraction()
    {
        EventManager.instance.ExitInteract();
        EventManager.instance.DisableSpinControls();
        PerspectiveLockManager.Instance.SetLock(false);
    }

    public string GetText()
    {
        return hoverText;
    }

}