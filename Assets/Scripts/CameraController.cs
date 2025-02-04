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

    public Transform zoomTarget; // The point to zoom to
    public static float perspectiveTransitionSpeed = 1f; // To perspective
    private float orthographicTargetSize = 0.4f; // Target orthographic size
    private float orthographicTransitionSpeed = 1f; //to orthographic
    private Vector3 initialPosition;
    private float initialOrthographicSize;
    private Vector3 targetPosition;

    private bool isZooming = false;
    private CinemachineBrain brain;

    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        EventManager.instance.OnToggleFirstPerson += StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD += TransitionToOrthographic;
        // Initial and target values for position and orthographic size
        initialPosition = orthographicCamera.transform.position;
        targetPosition = new Vector3(zoomTarget.position.x, zoomTarget.position.y, initialPosition.z); // Keep Z fixed for 2D camera
        initialOrthographicSize = orthographicCamera.m_Lens.OrthographicSize;
    }

    private void StartZoomInOutTransition()
    {
        if (!isZooming)
        {
            TransitionToOrthographicZoom();
            //StartCoroutine(TransitionToFirstPerson());
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
    }

    private void TransitionToPerspectiveCamera()
    {
        brain.m_DefaultBlend.m_Time = 0f;  // Adjust blend duration
        orthographicCamera.Priority = 0;
        // Set the second perspective camera as active to start blending between both
        perspectiveCamera.Priority = 1;
        orthographicCamera.transform.position = initialPosition;
        orthographicCamera.m_Lens.OrthographicSize = initialOrthographicSize;
    }
    public void TransitionToOrthographic()
    {
        brain.m_DefaultBlend.m_Time = orthographicTransitionSpeed;  // Adjust blend duration
        perspectiveCatchCamera.Priority = 1;
        perspectiveCamera.Priority = 0;
        StartCoroutine(WaitToTransitionToOrthographic());
    }

    private IEnumerator WaitToTransitionToOrthographic()
    {
        yield return new WaitForSeconds(orthographicTransitionSpeed);
        brain.m_DefaultBlend.m_Time = 0f;  // Adjust blend duration
        perspectiveCatchCamera.Priority = 0;
        orthographicCamera.Priority = 1;
    }
}
