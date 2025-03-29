using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class InputManager : MonoBehaviour
{
    private bool isTwoD = false;
    
    // link to the pause menu
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        StartCoroutine(WaitToInstantiateGamePlay());
    }
    
    //So that block 
    private IEnumerator WaitToInstantiateGamePlay()
    {
        yield return null;
        
        EventManager.instance.InstantiateGamePlay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //going to First Person
            if(isTwoD)
            {            
                EventManager.instance.ToggleFirstPerson();
                isTwoD = false;
                
            }
            //going to Two D
            else
            {
                EventManager.instance.ToggleTwoD();
                isTwoD = true;
                
            }
            // TogglePerspective();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // show pause menu
            pauseMenu.SetActive(true);
            // Re-enable the cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            EventManager.instance.SetPauseMenu(true);
            EventManager.instance.PauseGamePlay(true);
        }
    }
}
