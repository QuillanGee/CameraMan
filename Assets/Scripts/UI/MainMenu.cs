using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public Text subtitlesButtonText; 
    public void Start()
    {
        Cursor.visible = true;
    }

    public void OnClickLoadScene()
    {
        //load the first level
        // FindObjectOfType<FlashTransition>().Flash();
        
        SceneManager.LoadScene("Wake Up Clean");

    }

    public void OnClickSettings()
    {
        // Hide the main menu
        mainMenu.SetActive(false);
        // Show the settings menu
        settingsMenu.SetActive(true);
    }

    public void OnClickSettingsBack()
    {
        // Hide the main menu
        mainMenu.SetActive(true);
        // Show the settings menu
        settingsMenu.SetActive(false);
    }
    
    public void OnClickSubtitles()
    {
        // Toggle the subtitles
        if (SubtitleToggle.Instance.check)
        {
            subtitlesButtonText.text = "Subtitles: Off";
        }
        else
        {
            subtitlesButtonText.text = "Subtitles: On";
        }
        SubtitleToggle.Instance.check = !SubtitleToggle.Instance.check;

        // Update the button text
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