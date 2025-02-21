using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public float climbSpeed = 5.0f; // Speed at which the player climbs the ladder
    private bool isClimbing = false; // Whether the player is currently climbing the ladder
    public FirstPersonCharacterMovement playerMovement; // Reference to the player's movement script
    public Rigidbody playerRigidbody; // Reference to the player's Rigidbody
    public Transform playerTransform; // Reference to the player's transform
    public float detectionRadius = 1.0f; // Radius to detect the player

    void Update()
    {
        CheckForPlayer();
        if (isClimbing)
        {
            ClimbLadder();
        }
    }

    private void CheckForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        bool playerDetected = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                playerDetected = true;
                if (!isClimbing)
                {
                    StartClimbing();
                }
                break;
            }
        }

        if (!playerDetected && isClimbing)
        {
            StopClimbing();
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        playerMovement.enabled = false; // Disable the player's normal movement
        playerRigidbody.useGravity = false; // Disable gravity while climbing
        playerRigidbody.velocity = Vector3.zero; // Reset velocity to prevent sliding
    }

    private void StopClimbing()
    {
        isClimbing = false;
        playerMovement.enabled = true; // Re-enable the player's normal movement
        playerRigidbody.useGravity = true; // Re-enable gravity
    }

    private void ClimbLadder()
    {
        float verticalInput = Input.GetAxis("Vertical");

        // Move the player up or down the ladder based on input
        Vector3 climbDirection = new Vector3(0, verticalInput, 0);
        playerRigidbody.velocity = climbDirection * climbSpeed;
    }
}