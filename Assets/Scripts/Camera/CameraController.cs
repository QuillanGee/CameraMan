using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera orthographicCamera;
    public CinemachineVirtualCamera perspectiveCamera;

    public static float perspectiveTransitionSpeed = 1f; // To perspective
    private float orthographicTransitionSpeed = 1f; //to orthographic

    private bool isZooming = false;
    private CinemachineBrain brain;

    // private void Awake()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }
    
    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        EventManager.instance.OnToggleFirstPerson += StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD += TransitionToOrthographic;
        GameObject orthographicCameraInScene = GameObject.FindWithTag("OrthographicCamera");
        orthographicCamera = orthographicCameraInScene.GetComponent<CinemachineVirtualCamera>();
    }

    private void StartZoomInOutTransition()
    {
        if (!isZooming)
        {
            EventManager.instance.PauseGamePlay(true);  // Pause mechanics
            StartCoroutine(WaitForOrthographicZoom());

        }
    }


    private IEnumerator WaitForOrthographicZoom()
    {
        yield return new WaitForSeconds(perspectiveTransitionSpeed);
        TransitionToPerspectiveCamera();
        EventManager.instance.PauseGamePlay(false);  // Resume mechanics
    }

    private void TransitionToPerspectiveCamera()
    {
        EventManager.instance.PostToggleFirstPerson();
        brain.m_DefaultBlend.m_Time = 0f;  // Adjust blend duration
        orthographicCamera.Priority = 0;
        
        // Set the second perspective camera as active to start blending between both
        perspectiveCamera.Priority = 1;
    }
    private void TransitionToOrthographic()
    {
        EventManager.instance.PauseGamePlay(true);  // Resume mechanics
        brain.m_DefaultBlend.m_Time = orthographicTransitionSpeed;  // Adjust blend duration

        StartCoroutine(WaitToTransitionToOrthographic());
    }

    private IEnumerator WaitToTransitionToOrthographic()
    {
        yield return new WaitForSeconds(orthographicTransitionSpeed);
        EventManager.instance.PostToggleTwoD();
        brain.m_DefaultBlend.m_Time = 0f;  // Adjust blend duration
        
        perspectiveCamera.Priority = 0;
        orthographicCamera.Priority = 1;
        EventManager.instance.PauseGamePlay(false);  // Resume mechanics
    }
}
