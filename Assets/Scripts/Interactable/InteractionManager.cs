using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public float interactionRange = 2f;

    public Image reticle; // Reference to the reticle UI element
    public Sprite defaultReticle; // Default reticle sprite
    public Sprite openHandReticle; // Open hand reticle sprite
    public Sprite closedHandReticle; // Closed hand reticle sprite

    private IInteractable currentHoveredObject;
    private PickableObject heldObject;
    [SerializeField] Transform holdPosition; // Where the block will be held when picked up
    [SerializeField] private Collider holdPosCollider; // The collider of the hold position
    
    public TMP_Text hoverText;

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
            if (heldObject == null && currentHoveredObject is PickableObject pickable)
            {
                pickable.Pickup(holdPosition);
                heldObject = pickable;
                holdPosCollider.enabled = true;
                EventManager.instance.HoldingBlock();
                
                // Change to closed hand when picking up
                reticle.sprite = closedHandReticle;
            }
            else if (heldObject == null && currentHoveredObject is InteractableObject interactable)
            {
                interactable.OnInteract();
            }
            else if (heldObject != null && currentHoveredObject is InteractableObject interactable1) // Drop if already holding something
            {
                interactable1.OnExitInteraction();
            }
            else if (heldObject != null && currentHoveredObject is PickableObject pickable1) // Drop if already holding something
            {
                heldObject.Drop();
                heldObject = null;
                holdPosCollider.enabled = false;
                EventManager.instance.NotHoldingBlock();

                // Change back to open hand if still hovering over something, otherwise reset
                reticle.sprite = currentHoveredObject != null ? openHandReticle : defaultReticle;
            }
        }
    }

}