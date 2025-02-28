using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MainMenu : MonoBehaviour
{
    public void OnClickLoadScene()
    {
        //load the first level
        FindObjectOfType<FlashTransition>().Flash();

    }


    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Stops play mode in editor
        #else
            Application.Quit();  // Quits the built application
        #endif

    }
}