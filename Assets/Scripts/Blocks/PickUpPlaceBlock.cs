using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PickUpPlaceBlock : MonoBehaviour
{
    public LayerMask blockLayer; // Layer that defines which objects can be picked up
    public float pickupDistance = 2f; // Maximum distance to pick up objects
    public Transform holdPosition; // Where the block will be held when picked up
    public bool isHolding = false;

    public Image reticle; // Reference to the reticle UI element
    public Sprite defaultReticle; // Default reticle sprite
    public Sprite openHandReticle; // Open hand reticle sprite
    public Sprite closedHandReticle; // Closed hand reticle sprite

    private GameObject player;
    [SerializeField] private Collider holdPosCollider; // The collider of the hold position

    private GameObject pickedBlock = null; // The currently picked-up block

    void Start()
    {
        
    }

    void Update()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            // If already holding a block, place it
            if (pickedBlock)
            {
                PlaceBlock();
            }
            else
            {
                // Otherwise, try to pick up a block
                PickUpBlock();
            }
        }

        // Update the reticle based on what the player is looking at
        UpdateReticle();
    }

    // Method to pick up the block
    void PickUpBlock()
    {
        // Raycast from the center of the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, blockLayer))
        {
            if (hit.collider != null && hit.collider.CompareTag("PickUp"))
            {
                // If we hit a block, pick it up
                pickedBlock = hit.collider.gameObject;
                isHolding = true;
                pickedBlock.GetComponent<Rigidbody>().isKinematic = true;
                pickedBlock.GetComponent<Collider>().enabled = false;
                holdPosCollider.GetComponent<Collider>().enabled = true;
                pickedBlock.transform.SetParent(holdPosition);
                HoldBlock();

                // Change reticle to closed hand
                reticle.sprite = closedHandReticle;
            }
        }
    }

    // Method to hold the block in front of the player or camera
    void HoldBlock()
    {
        // Move the block to the hold position (e.g., in front of the camera)
        pickedBlock.transform.position = holdPosition.position;
        pickedBlock.transform.rotation = holdPosition.rotation;
    }

    // Method to place the block
    void PlaceBlock()
    {
        isHolding = false;
        // Enable physics again for the block
        pickedBlock.GetComponent<Rigidbody>().isKinematic = false;
        pickedBlock.GetComponent<Collider>().enabled = true;
        holdPosCollider.GetComponent<Collider>().enabled = false;
        pickedBlock.transform.parent = null;
        pickedBlock = null; // Clear the reference to the block

        // Change reticle to default
        reticle.sprite = defaultReticle;
    }

    // Method to update the reticle based on what the player is looking at
    void UpdateReticle()
    {
        if (isHolding)
        {
            return; // Do not change reticle if holding a block
        }

        // Raycast from the center of the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, blockLayer))
        {
            if (hit.collider != null && hit.collider.CompareTag("PickUp"))
            {
                // Change reticle to open hand
                reticle.sprite = openHandReticle;
                return;
            }
        }

        // Change reticle to default
        reticle.sprite = defaultReticle;
    }
}