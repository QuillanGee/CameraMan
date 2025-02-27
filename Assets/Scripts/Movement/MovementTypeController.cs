using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeController : MonoBehaviour
{
    
    [SerializeField] private FirstPersonCharacterMovement firstPersonCharacterMovement;
    [SerializeField] private TwoDCharacterMovement twoDCharacterMovement;
    void Start()
    {
        EventManager.instance.OnToggleFirstPerson += ToggleControlsForFirstPerson;
        EventManager.instance.OnToggleTwoD += ToggleControlsForTwoD;
    }
    private void ToggleControlsForTwoD()
    {
        twoDCharacterMovement.enabled = true;
        firstPersonCharacterMovement.enabled = false;
    }

    private void ToggleControlsForFirstPerson()
    {
        firstPersonCharacterMovement.enabled = true;
        twoDCharacterMovement.enabled = false;
    }
}
