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
                if (currentHoveredObject != interactable)
                {
                    if (currentHoveredObject != null)
                        currentHoveredObject.OnHoverExit();

                    currentHoveredObject = interactable;
                    currentHoveredObject.OnHoverEnter();
                    reticle.sprite = openHandReticle;
                }
            }
            else
            {
                reticle.sprite = defaultReticle;
            }
        }
        else
        {
            if (currentHoveredObject != null)
            {
                currentHoveredObject.OnHoverExit();
                currentHoveredObject = null;
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
            }
            else if (heldObject != null) // Drop if already holding something
            {
                heldObject.Drop();
                heldObject = null;
                holdPosCollider.enabled = false;
                EventManager.instance.NotHoldingBlock();
            }
        }
    }
}