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
            }
        }
    }
}