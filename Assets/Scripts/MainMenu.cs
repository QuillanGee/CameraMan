using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void OnClickLoadScene()
    {
        //load the first level
        FindObjectOfType<FlashTransition>().Flash();
        SceneManager.LoadScene("Level 4");


    }


    public void OnClickQuit()
    {
        //uncomment to quit from build
        // Application.Quit();
      
        //comment out after creating build
        UnityEditor.EditorApplication.isPlaying = false;


    }
}