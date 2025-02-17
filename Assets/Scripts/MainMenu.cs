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
        //uncomment to quit from build
        // Application.Quit();
      
        //comment out after creating build
        UnityEditor.EditorApplication.isPlaying = false;


    }
}