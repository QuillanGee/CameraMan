using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan : MonoBehaviour
{
    [SerializeField] GameObject Alan2D;
    private Canvas crossHair;
    private Vector3 startingPosition;

    
    private void Start()
    {
        crossHair = GetComponentInChildren<Canvas>();
        startingPosition = transform.position;

        EventManager.instance.OnToggleFirstPerson += ProjectAlan2DToMoveAlan;
        EventManager.instance.OnToggleTwoD += DisableCrossHair;
        EventManager.instance.OnToggleFirstPerson += WaitToEnableCrossHair;
        EventManager.instance.OnResetAlan += ResetPosition;
    }
    
    // Start is called before the first frame update
    
    public void ProjectAlan2DToMoveAlan()
    {
        //Moves to corresponding X position
        Vector3 newPositionX = transform.position;
        newPositionX.x = Alan2D.transform.position.x;
        transform.position = newPositionX;
        
        //Moves to corresponding X position
        Vector3 newPositionY = transform.position;
        newPositionY.y = Alan2D.transform.position.y;
        transform.position = newPositionY;
        
    }
    
    private void DisableCrossHair()
    {
        crossHair.enabled = false;
    }

    private void WaitToEnableCrossHair()
    {
        StartCoroutine(WaitToEnableCrossHairCoroutine());
    }
    
    private IEnumerator WaitToEnableCrossHairCoroutine()
    {
        yield return new WaitForSeconds(CameraController.perspectiveTransitionSpeed);
        crossHair.enabled = true;
    }

    private void ResetPosition()
    {
        transform.position = startingPosition;
    }
}
