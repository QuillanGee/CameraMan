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
            if (!isInteracting)
            {
                isInteracting = true;
                if (currentHoveredObject is PickableObject pickable)
                {
                    //pickup logic
                    pickable.Pickup(holdPosition);
                    heldObject = pickable;
                    holdPosCollider.enabled = true;
                    EventManager.instance.HoldingBlock();
                    reticle.sprite = closedHandReticle;
                }
                else if (currentHoveredObject is InteractableObject interactable)
                {
                    interactable.OnInteract();
                }
            }
            else
            {
                isInteracting = false;
                if (currentHoveredObject is InteractableObject interactable1)
                {
                    interactable1.OnExitInteraction();
                }
                else if (heldObject != null)
                {
                    heldObject.Drop();
                    heldObject = null;
                    holdPosCollider.enabled = false;
                    EventManager.instance.NotHoldingBlock();
                    reticle.sprite = currentHoveredObject != null ? openHandReticle : defaultReticle;
                    isInteracting = false;
                }
            }
        }
    }

}