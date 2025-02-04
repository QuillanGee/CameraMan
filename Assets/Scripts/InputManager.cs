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
    // [SerializeField] private GameObject currProjected3DObject;
    // [SerializeField] private ObjectProjection objectProjection;
    // [SerializeField] private CameraController cameraController;
    // [SerializeField] private GameObject alan;
    // [SerializeField] private GameObject alan2D;
    
    //keep this for logic purposes
    [SerializeField] private PickUpPlaceBlock pickUpPlaceBlock;
    private bool isTwoD = true;

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
                
                //check if holding block, if is then toggle hold block event (Maybe check if there is a block)
                
                if (pickUpPlaceBlock.isHolding)
                {
                    EventManager.instance.HoldingBlock();
                }
                else
                {
                    EventManager.instance.NotHoldingBlock();
                }
            }
            // TogglePerspective();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;  // Stops play mode in editor
            #else
                        Application.Quit();  // Quits the built application
            #endif
        }
    }
}
