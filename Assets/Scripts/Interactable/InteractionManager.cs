using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    public Image reticle; // Reference to the reticle UI element
    public Sprite defaultReticle; // Default reticle sprite
    public Sprite rightClickReticle; // Open hand reticle sprite
    // public Sprite closedHandReticle; // Closed hand reticle sprite
    private IInteractable currentHoveredObject;
    private PickableObject heldObject;
    private PickableObject heldObject2D;
    [SerializeField] Transform holdPosition; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionDoor; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionWhiteboard; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionDuct; // Where the block will be held when picked up
    [SerializeField] private Collider holdPosCollider; // The collider of the hold position
    
    public TMP_Text hoverText;

    public bool isInteracting = false;

    public bool showUI = true;
    
    public bool is2D = false;


    void Start()
    {
        EventManager.instance.OnToggleFirstPerson += EnableFirstPersonInteraction;
        EventManager.instance.OnToggleTwoD += DisableFirstPersonInteraction;
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
            // 3D Hover using Raycasting
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
                    reticle.sprite = rightClickReticle;
                    reticle.transform.localScale = new Vector3(5, 5, 5);
                    // hoverText.text = interactable.GetText();
                }
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
        reticle.sprite = defaultReticle;
        reticle.transform.localScale = new Vector3(1, 1, 1);
        // hoverText.text = "";


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
                    reticle.enabled = false;
                    isInteracting = true;
                    // Pickup logic for 2D
                    if (pickable.gameObject.CompareTag("Door"))
                    {
                        pickable.Pickup(holdPositionDoor);
                    }
                    else if (pickable.gameObject.CompareTag("Walls"))
                    {
                        pickable.Pickup(holdPositionWhiteboard);
                    }
                    else if (pickable.gameObject.CompareTag("Duct"))
                    {
                        pickable.Pickup(holdPositionDuct);
                    }
                    else
                    {
                        pickable.Pickup(holdPosition);
                    }
                    heldObject = pickable;
                    holdPosCollider.enabled = true;
                    EventManager.instance.HoldingBlock();
                }
                else if (currentHoveredObject is InteractableObject interactable)
                {
                    isInteracting = true;
                    reticle.enabled = false;
                    interactable.OnInteract();
                }
                else if (currentHoveredObject is Spinable spinable)
                {
                    isInteracting = true;
                    reticle.enabled = false;
                    spinable.OnInteract();
                }
            }
            else
            {
                isInteracting = false;
                reticle.enabled = true;
                if (currentHoveredObject is InteractableObject interactable1)
                {
                    interactable1.OnExitInteraction();
                }
                if (heldObject != null)
                {
                    heldObject.Drop();
                    heldObject = null;
                    holdPosCollider.enabled = false;
                    EventManager.instance.NotHoldingBlock();
                    isInteracting = false;
                }
                else if (currentHoveredObject is Spinable spinable1)
                {
                    spinable1.OnExitInteraction();
                }
            }
        }
    }

    private void DisableFirstPersonInteraction()
    {
        gameObject.SetActive(false);
    }

    private void EnableFirstPersonInteraction()
    {
        gameObject.SetActive(true);
    }
}