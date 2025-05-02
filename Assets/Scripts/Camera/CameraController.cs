using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera orthographicCamera;
    [SerializeField] CinemachineVirtualCamera perspectiveCamera;
    [SerializeField] CinemachineVirtualCamera beginningAnimationCamera;
    [SerializeField] private float glitchEffectDuration = 0.8f; // Duration of the glitch effect
    private CinemachineVirtualCamera interactionCamera;

    private float perspectiveTransitionSpeed = 0.5f; // To perspective
    private float orthographicTransitionSpeed = 0.5f; //to orthographic
    private float beginningTransitionSpeed = 3f;
    
    [SerializeField] private bool animateOnStart = true;

    private CinemachineBrain brain;
    
    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        EventManager.instance.OnToggleFirstPerson += StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD += TransitionToOrthographic;
        EventManager.instance.OnLoadScene += AttachOrthographicCamera;
        EventManager.instance.OnHover += LinkInteractionCamera;
        EventManager.instance.OnInteract += TransitionToInteractionCamera;
        EventManager.instance.OnExitInteract += TransitionFromInteractionCamera;
        EventManager.instance.OnToggleFirstPerson += EditCullingMaskGoingToFirstPerson;
        EventManager.instance.OnToggleTwoD += EditCullingMaskGoingToTwoDPerson;
        
        
        if (animateOnStart)
        {
            beginningAnimationCamera.Priority = 1;
            perspectiveCamera.Priority = 0;
            StartCoroutine(StartBeginningAnimation());
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.OnToggleFirstPerson -= StartZoomInOutTransition;
        EventManager.instance.OnToggleTwoD -= TransitionToOrthographic;
        EventManager.instance.OnLoadScene -= AttachOrthographicCamera;
        EventManager.instance.OnHover -= LinkInteractionCamera;
        EventManager.instance.OnInteract -= TransitionToInteractionCamera;
        EventManager.instance.OnExitInteract -= TransitionFromInteractionCamera;
        EventManager.instance.OnToggleFirstPerson -= EditCullingMaskGoingToFirstPerson;
        EventManager.instance.OnToggleTwoD -= EditCullingMaskGoingToTwoDPerson;
    }

    private IEnumerator StartBeginningAnimation()
    {
        yield return new WaitForSeconds(2f);
        brain.m_DefaultBlend.m_Time = beginningTransitionSpeed;
        beginningAnimationCamera.Priority = 0;
        perspectiveCamera.Priority = 1;
    }

    private void EditCullingMaskGoingToFirstPerson()
    {
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("TVs");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Blocks");
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Gone3D"));
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Player")); // Hide player in perspective view
    }

    private void EditCullingMaskGoingToTwoDPerson()
    {
        // Only update culling mask if we're actually in 2D mode
        if (orthographicCamera.Priority > perspectiveCamera.Priority)
        {
            // Keep TVs visible during transition
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Blocks"));
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Gone3D");
        }
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
        EventManager.instance.PauseGamePlay(true);  // Pause mechanics
        brain.m_DefaultBlend.m_Time = 0f;  // Set blend time to 0 to prevent blending
        perspectiveCamera.Priority = 1;  // Ensure perspective camera stays active
        orthographicCamera.Priority = 0;  // Ensure orthographic camera is inactive
        
        // Ensure player and TVs are visible at the start of transition
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("TVs");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        
        brain.enabled = false;  // Freeze the camera's image
        StartCoroutine(WaitForGlitchEffect());
    }

    private IEnumerator WaitForGlitchEffect()
    {
        // Wait for the glitch effect to play
        yield return new WaitForSeconds(glitchEffectDuration);
        
        // After glitch effect, switch to orthographic camera
        brain.enabled = true;  // Re-enable the brain
        perspectiveCamera.Priority = 0;
        orthographicCamera.Priority = 1;
        
        // Update culling mask after the transition
        EditCullingMaskGoingToTwoDPerson();
        
        // Hide TVs after the transition is complete
        yield return new WaitForEndOfFrame();
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("TVs"));
        
        EventManager.instance.PostToggleTwoD();
        EventManager.instance.PauseGamePlay(false);  // Resume mechanics
    }

    private void LinkInteractionCamera(object sender, CinemachineVirtualCamera cameraInteraction)
    {
        interactionCamera = cameraInteraction;
    }

    private void TransitionToInteractionCamera()
    {
        if (interactionCamera != null)
        {
            brain.m_DefaultBlend.m_Time = 1f;  // Adjust blend duration
            perspectiveCamera.Priority = 0;
            interactionCamera.Priority = 1;
        }
        else
        {
            print("No interaction camera");
        }
        
    }

    private void TransitionFromInteractionCamera()
    {
        brain.m_DefaultBlend.m_Time = 1f;  // Adjust blend duration
        interactionCamera.Priority = 0;
        perspectiveCamera.Priority = 1;
    }
}
