using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    public Image reticle; // Reference to the reticle UI element
    public Sprite defaultReticle; // Default reticle sprite
    public Sprite openHandReticle; // Open hand reticle sprite
    public Sprite closedHandReticle; // Closed hand reticle sprite

    private IInteractable currentHoveredObject;
    private PickableObject heldObject;
    [SerializeField] Transform holdPosition; // Where the block will be held when picked up
    [SerializeField] private Collider holdPosCollider; // The collider of the hold position
    
    public TMP_Text hoverText;

    private bool isInteracting = false;

    void Update()
    {
        HandleHover();
        HandleInteraction();
    }

    private void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                // If looking at a new object, reset previous and set new hover state
                if (currentHoveredObject != interactable)
                {
                    if (currentHoveredObject != null)
                        currentHoveredObject.OnHoverExit();

                    currentHoveredObject = interactable;
                    currentHoveredObject.OnHoverEnter();
                    hoverText.text = interactable.GetText();
                }

                // Only change reticle if we are NOT holding something
                if (heldObject == null)
                {
                    reticle.sprite = openHandReticle;
                }
            }
            else
            {
                // If looking at nothing, reset hover state
                if (currentHoveredObject != null)
                {
                    currentHoveredObject.OnHoverExit();
                    currentHoveredObject = null;
                    hoverText.text = "";
                }

                // Only change to default reticle if NOT holding something
                if (heldObject == null)
                {
                    reticle.sprite = defaultReticle;
                }
            }
        }
        else
        {
            // If raycast hits nothing, reset hover state
            if (currentHoveredObject != null)
            {
                currentHoveredObject.OnHoverExit();
                currentHoveredObject = null;
                hoverText.text = "";
            }

            // Only change to default reticle if NOT holding something
            if (heldObject == null)
            {
                reticle.sprite = defaultReticle;
            }
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isInteracting && currentHoveredObject is PickableObject pickable)
            {
                AudioManager.instance.PlayBlockPickUp();
                pickable.Pickup(holdPosition);
                heldObject = pickable;
                holdPosCollider.enabled = true;
                EventManager.instance.HoldingBlock();
                // Change to closed hand when picking up
                reticle.sprite = closedHandReticle;
                isInteracting = true;
            }
            else if (!isInteracting && currentHoveredObject is InteractableObject interactable)
            {
                interactable.OnInteract();
                isInteracting = true;
            }
            else if (isInteracting && currentHoveredObject is InteractableObject interactable1) // Drop if already holding something
            {
                interactable1.OnExitInteraction();
                isInteracting = false;

            }
            else if (isInteracting && heldObject != null) // Drop if already holding something
            {
                AudioManager.instance.PlayBlockPickUp();
                heldObject.Drop();
                heldObject = null;
                holdPosCollider.enabled = false;
                EventManager.instance.NotHoldingBlock();

                // Change back to open hand if still hovering over something, otherwise reset
                reticle.sprite = currentHoveredObject != null ? openHandReticle : defaultReticle;
                isInteracting = false;
                print("Block dropped");
            }
        }
    }

}