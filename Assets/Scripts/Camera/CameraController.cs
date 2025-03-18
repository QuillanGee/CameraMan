using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera orthographicCamera;
    public CinemachineVirtualCamera perspectiveCamera;

    private float perspectiveTransitionSpeed = 1f; // To perspective
    private float orthographicTransitionSpeed = 1f; //to orthographic

    private CinemachineBrain brain;
    
    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        EventManager.instance.OnToggleFirstPerson += StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD += TransitionToOrthographic;
        EventManager.instance.OnLoadScene += AttachOrthographicCamera;
        AttachOrthographicCamera();
    }

    private void OnDestroy()
    {
        EventManager.instance.OnToggleFirstPerson -= StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD -= TransitionToOrthographic;
        EventManager.instance.OnLoadScene -= AttachOrthographicCamera;
    }

    private void AttachOrthographicCamera()
    {
        orthographicCamera = GameObject.FindWithTag("OrthographicCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void StartZoomInOutTransition()
    {
        EventManager.instance.PauseGamePlay(true);  // Pause mechanics
        brain.m_DefaultBlend.m_Time = perspectiveTransitionSpeed;  // Adjust blend duration
        StartCoroutine(WaitForOrthographicZoom());
    }


    private IEnumerator WaitForOrthographicZoom()
    {
        yield return new WaitForSeconds(perspectiveTransitionSpeed);
        TransitionToPerspectiveCamera();
    }

    private void TransitionToPerspectiveCamera()
    {
        EventManager.instance.PostToggleFirstPerson();
        brain.m_DefaultBlend.m_Time = 0f;  // Adjust blend duration
        orthographicCamera.Priority = 0;
        perspectiveCamera.Priority = 1;
        EventManager.instance.PauseGamePlay(false);  // Resume mechanics
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
