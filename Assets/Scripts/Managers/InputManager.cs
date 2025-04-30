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
    [SerializeField] private Text errorMessageUI;
    [SerializeField] private float errorDisplayTime = 1f;
    [SerializeField] private GameObject Subtitles;


    void Start()
    {
        StartCoroutine(WaitToInstantiateGamePlay());
        // if (SubtitleToggle.Instance.check == false)
        // {
        //     Subtitles.SetActive(false);
        // }
        // else
        // {
        //     Subtitles.SetActive(true);
        // }
    }
    
    //So that block 
    private IEnumerator WaitToInstantiateGamePlay()
    {
        yield return null;
        
        EventManager.instance.InstantiateGamePlay();
        EventManager.instance.OnSendError += PlayErrorFeedback;
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
                    PlayErrorFeedback(this, "Obstruction Detected");
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
    
    private void PlayErrorFeedback(object sender, String errorFeedback)
    {
        if (audioSource != null && errorSound != null)
        {
            audioSource.PlayOneShot(errorSound);
        }

        if (errorMessageUI != null)
        {
            StopAllCoroutines(); // prevent overlap if spammed
            print("set text");
            errorMessageUI.text = errorFeedback;
            StartCoroutine(HideErrorMessage());
        }
    }

    private IEnumerator HideErrorMessage()
    {
        yield return new WaitForSeconds(errorDisplayTime);
        errorMessageUI.text = "";
    }

}
