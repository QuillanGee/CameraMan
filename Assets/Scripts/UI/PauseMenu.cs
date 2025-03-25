using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        gameObject.SetActive(false);
    }
    
    // button press
    public void OnClickResume()
    {
        // show pause menu
        this.gameObject.SetActive(false);
        // Re-enable the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        EventManager.instance.SetPauseMenu(false);
        EventManager.instance.PauseGamePlay(false);
        EventSystem.current.SetSelectedGameObject(null);
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
