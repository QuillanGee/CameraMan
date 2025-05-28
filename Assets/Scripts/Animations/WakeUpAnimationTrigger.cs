using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WakeUpAnimationTrigger : MonoBehaviour
{
    private Animator animator;
    private CinemachineVirtualCamera fpc;

    void Start()
    {
        animator = GetComponent<Animator>();
        fpc = GetComponent<CinemachineVirtualCamera>();
    }
    
    
    private void PlayOpeningAnimation()
    {
        animator.enabled = false;
        fpc.enabled = true;
    }
}
