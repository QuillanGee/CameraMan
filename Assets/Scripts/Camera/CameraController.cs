using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera orthographicCamera;
    public CinemachineVirtualCamera orthographicZoom;
    public CinemachineVirtualCamera perspectiveCamera;
    public CinemachineVirtualCamera perspectiveCatchCamera;

    public static float perspectiveTransitionSpeed = 1f; // To perspective
    private float orthographicTransitionSpeed = 1f; //to orthographic
    private float initialOrthographicSize;

    private bool isZooming = false;
    private CinemachineBrain brain;

    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        EventManager.instance.OnToggleFirstPerson += StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD += TransitionToOrthographic;
        // Initial and target values for position and orthographic size
        initialOrthographicSize = orthographicCamera.m_Lens.OrthographicSize;
    }

    private void StartZoomInOutTransition()
    {
        if (!isZooming)
        {
            EventManager.instance.PauseGamePlay(true);  // Pause mechanics
            // TransitionToOrthographicZoom();
            StartCoroutine(WaitForOrthographicZoom());

        }
    }

    private void TransitionToOrthographicZoom()
    {
        brain.m_DefaultBlend.m_Time = perspectiveTransitionSpeed;  // Adjust blend duration
        orthographicZoom.Priority = 1;
        orthographicCamera.Priority = 0;
        StartCoroutine(WaitForOrthographicZoom());
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
        // orthographicZoom.Priority = 0;
        orthographicCamera.Priority = 0;
        // Set the second perspective camera as active to start blending between both
        perspectiveCamera.Priority = 1;
        orthographicCamera.m_Lens.OrthographicSize = initialOrthographicSize;
    }
    private void TransitionToOrthographic()
    {
        EventManager.instance.PauseGamePlay(true);  // Resume mechanics
        brain.m_DefaultBlend.m_Time = orthographicTransitionSpeed;  // Adjust blend duration
        // perspectiveCatchCamera.Priority = 1;
        // perspectiveCamera.Priority = 0;
        StartCoroutine(WaitToTransitionToOrthographic());
    }

    private IEnumerator WaitToTransitionToOrthographic()
    {
        yield return new WaitForSeconds(orthographicTransitionSpeed);
        EventManager.instance.PostToggleTwoD();
        brain.m_DefaultBlend.m_Time = 0f;  // Adjust blend duration
        
        // perspectiveCatchCamera.Priority = 0;
        perspectiveCamera.Priority = 0;
        orthographicCamera.Priority = 1;
        EventManager.instance.PauseGamePlay(false);  // Resume mechanics
    }
}
