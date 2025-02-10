using UnityEngine;
using Cinemachine;

public class PickUpPlaceBlock : MonoBehaviour
{
    public LayerMask blockLayer; // Layer that defines which objects can be picked up
    public float pickupDistance = 2f; // Maximum distance to pick up objects
    public Transform holdPosition; // Where the block will be held when picked up
    public bool isHolding = false;
    
    private GameObject pickedBlock = null; // The currently picked-up block

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
    }

    // Method to pick up the block
    void PickUpBlock()
    {
        // Raycast from the center of the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, blockLayer))
        {
            // If we hit a block, pick it up
            pickedBlock = hit.collider.gameObject;
            isHolding = true;
            pickedBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;;
            pickedBlock.GetComponent<Rigidbody>().isKinematic = true;
            pickedBlock.transform.SetParent(holdPosition);
            HoldBlock();
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
        pickedBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        pickedBlock.GetComponent<Rigidbody>().isKinematic = false;
        pickedBlock.transform.parent = null;
        pickedBlock = null; // Clear the reference to the block
    }
}