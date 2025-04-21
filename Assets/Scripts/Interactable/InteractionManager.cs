using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    // public Image reticle; // Reference to the reticle UI element
    // public Sprite defaultReticle; // Default reticle sprite
    // public Sprite openHandReticle; // Open hand reticle sprite
    // public Sprite closedHandReticle; // Closed hand reticle sprite

    private IInteractable currentHoveredObject;
    private PickableObject heldObject;
    [SerializeField] Transform holdPosition; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionDoor; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionWhiteboard; // Where the block will be held when picked up
    [SerializeField] private Collider holdPosCollider; // The collider of the hold position
    
    [SerializeField] private Image rightClickImage;
    public TMP_Text hoverText;

    public bool isInteracting = false;

    public bool showUI = true;

    void Start()
    {
        // if (showUI)
        // {
        //     reticle.gameObject.SetActive(true);
        // }
        // else
        // {
        //     reticle.gameObject.SetActive(false);
        // }
    }
    
    void Update()
    {
        if (showUI)
        {
            HandleHover();
            HandleInteraction();
        }
    }

    private void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, interactionRange);
    
        IInteractable interactable = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            IInteractable hitInteractable = hit.collider.GetComponent<IInteractable>();
            if (hitInteractable != null && hit.distance < closestDistance)
            {
                interactable = hitInteractable;
                closestDistance = hit.distance;
            }
        }

        if (interactable != null)
        {
            if (currentHoveredObject != interactable)
            {
                currentHoveredObject?.OnHoverExit();
                currentHoveredObject = interactable;
                currentHoveredObject.OnHoverEnter();
                hoverText.text = interactable.GetText();
                // rightClickImage.enabled = true;
            }

            // if (heldObject == null)
            //     reticle.sprite = openHandReticle;
        }
        else
        {
            ResetHoverState();
        }
    }

    private void ResetHoverState()
    {
        currentHoveredObject?.OnHoverExit();
        currentHoveredObject = null;
        hoverText.text = "";
        // rightClickImage.enabled = false;


        // if (heldObject == null)
        //     reticle.sprite = defaultReticle;
    }


    private void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isInteracting)
            {
                if (currentHoveredObject is PickableObject pickable)
                {
                    isInteracting = true;
                    //pickup logic
                    if (pickable.gameObject.CompareTag("Door"))
                    {
                        pickable.Pickup(holdPositionDoor);
                    }
                    else if (pickable.gameObject.CompareTag("Walls"))
                    {
                        pickable.Pickup(holdPositionWhiteboard);
                    }
                    else
                    {
                        pickable.Pickup(holdPosition);
                    }
                    heldObject = pickable;
                    holdPosCollider.enabled = true;
                    EventManager.instance.HoldingBlock();
                    // reticle.sprite = closedHandReticle;
                }
                else if (currentHoveredObject is InteractableObject interactable)
                {
                    isInteracting = true;
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
                    // reticle.sprite = currentHoveredObject != null ? openHandReticle : defaultReticle;
                    isInteracting = false;
                }
            }
        }
    }

}