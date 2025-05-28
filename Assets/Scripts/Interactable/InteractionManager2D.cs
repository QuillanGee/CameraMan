using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager2D : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    
    private IInteractable currentHoveredObject;
    private Pickable2DObject heldObject2D;
    private bool isHolding = false;
    [SerializeField] Transform holdPosition; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionDoor; // Where the block will be held when picked up
    [SerializeField] Transform holdPositionWhiteboard; // Where the block will be held when picked up
    // [SerializeField] private Collider holdPosCollider2D; // The collider of the hold position
    
    [SerializeField] private Image rightClickImage;
    public TMP_Text hoverText;

    public bool isInteracting = false;

    public bool showUI = true;

    void Start()
    {
        EventManager.instance.OnToggleFirstPerson += Disable2DInteraction;
        EventManager.instance.OnToggleTwoD += Enable2DInteraction;
        EventManager.instance.OnToggleWhileHolding += SetPickup2DObject;
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
        if (currentHoveredObject != null)
        {
            hoverText.text = currentHoveredObject.GetText();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IInteractable hitInteractable = other.gameObject.GetComponent<IInteractable>();
        if (hitInteractable != null)
        {
            currentHoveredObject = hitInteractable;
            currentHoveredObject.OnHoverEnter();
            hoverText.text = currentHoveredObject.GetText();
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (currentHoveredObject != null)
        {
            currentHoveredObject.OnHoverExit();
            currentHoveredObject = null;
            hoverText.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable hitInteractable = other.GetComponent<IInteractable>();
        if (hitInteractable != null)
        {
            currentHoveredObject = hitInteractable;
            currentHoveredObject.OnHoverEnter();
            hoverText.text = currentHoveredObject.GetText();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentHoveredObject != null)
        {
            currentHoveredObject.OnHoverExit();
            currentHoveredObject = null;
            hoverText.text = "";
        }
    }
    
    
    // private void ResetHoverState()
    // {
    //     currentHoveredObject?.OnHoverExit();
    //     currentHoveredObject = null;
    //     hoverText.text = "";
    //     // rightClickImage.enabled = false;
    //
    //
    //     // if (heldObject == null)
    //     //     reticle.sprite = defaultReticle;
    // }


    private void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isInteracting)
            {
                if (currentHoveredObject is Pickable2DObject pickable2D)
                {
                    isInteracting = true;
                    // Pickup logic for 2D
                    if (pickable2D.gameObject.CompareTag("Door"))
                    {
                        pickable2D.Pickup(holdPositionDoor);
                    }
                    else if (pickable2D.gameObject.CompareTag("Walls"))
                    {
                        pickable2D.Pickup(holdPositionWhiteboard);
                    }
                    else
                    {
                        pickable2D.Pickup(holdPosition);
                    }
                    heldObject2D = pickable2D;
                    // holdPosCollider2D.enabled = true;
                    EventManager.instance.HoldingBlock();
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
                if (heldObject2D != null)
                {
                    heldObject2D.Drop();
                    heldObject2D = null;
                    // holdPosCollider2D.enabled = false;
                    EventManager.instance.NotHoldingBlock();
                    isInteracting = false;
                }
            }
        }
    }

    private void SetPickup2DObject()
    {
        heldObject2D = holdPosition.GetComponentInChildren<Pickable2DObject>();
        isInteracting = true;
    }
    

    private void Disable2DInteraction()
    {
        gameObject.SetActive(false);
    }

    private void Enable2DInteraction()
    {
        gameObject.SetActive(true);
    }
}