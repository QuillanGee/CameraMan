using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Key : MonoBehaviour
{
    public Door linkedDoor; // Assign the specific door in the Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Make sure the player has the correct tag
        {
            print("DoorUnlcoked");
            linkedDoor.UnlockDoor(); // Unlock the assigned door
            Destroy(gameObject); // Remove the key after pickup
        }
    }
}
