using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
    }

    public void OnClickLoadScene()
    {
        //load the first level
        // FindObjectOfType<FlashTransition>().Flash();
        
        SceneManager.LoadScene("Wake Up Clean");
        SceneManager.LoadScene("HallwayToWarehouse", LoadSceneMode.Additive);
        SceneManager.LoadScene("WarehouseLvClean", LoadSceneMode.Additive);

    }


    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Stops play mode in editor
        #else
            Application.Quit();  // Quits the built application
        #endif

    }
    
    // Used for testing purposes to end level
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