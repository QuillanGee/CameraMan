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
    
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject errorMessageUI;
    [SerializeField] private float errorDisplayTime = 1f;


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
                AudioManager.instance.PlayGlitchSoundEffect();
                isTwoD = false;
                
            }
            //going to Two D
            else
            {
                if (PerspectiveLockManager.Instance.IsLocked())
                {
                    PlayErrorFeedback();
                    return;
                }
                EventManager.instance.ToggleTwoD();
                AudioManager.instance.PlayGlitchSoundEffect();
                isTwoD = true;

            }
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
    
    private void PlayErrorFeedback()
    {
        if (audioSource != null && errorSound != null)
        {
            audioSource.PlayOneShot(errorSound);
        }

        if (errorMessageUI != null)
        {
            StopAllCoroutines(); // prevent overlap if spammed
            errorMessageUI.SetActive(true);
            StartCoroutine(HideErrorMessage());
        }
    }

    private IEnumerator HideErrorMessage()
    {
        yield return new WaitForSeconds(errorDisplayTime);
        errorMessageUI.SetActive(false);
    }

}
