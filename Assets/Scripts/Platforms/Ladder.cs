using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonCharacterMovement playerController = other.GetComponent<FirstPersonCharacterMovement>();
            if (playerController != null)
            {
                playerController.SetIsOnLadder(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonCharacterMovement playerController = other.GetComponent<FirstPersonCharacterMovement>();
            if (playerController != null)
            {
                playerController.SetIsOnLadder(false);

                // Move the player slightly upward to avoid re-triggering the ladder
                Vector3 pushDirection = transform.up * 1.0f; // Adjust push force as needed
                other.transform.position += pushDirection;
            }
        }
    }
}
    