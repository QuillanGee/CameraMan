using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure your player has the "Player" tag
        {

            // Re-enable the cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Load the MainMenu2 scene
            SceneManager.LoadScene("GameOver");
        }
    }
}
