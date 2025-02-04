using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlanProjectionController : MonoBehaviour
{
    [SerializeField] Transform projectedWallTransform;
    [SerializeField] GameObject Alan2D;
    [SerializeField] private Transform holdPosition2D;
    private Vector3 alanDefaultScale = new Vector3(3.04f,3.04f,3.04f);
    private ObjectProjection currentHeldObjectProjection;
    private Canvas crossHair;

    private void Start()
    {
        crossHair = GetComponentInChildren<Canvas>();
        
        EventManager.instance.OnToggleTwoD += ProjectAlan;
        EventManager.instance.OnToggleTwoD += DisableCrossHair;
        EventManager.instance.OnToggleFirstPerson += WaitToEnableCrossHair;
        EventManager.instance.OnHoldingBlock += SetObjectionProjectionInstance;
        EventManager.instance.OnHoldingBlock += AttachBlockToAlan2D;
    }
    
    public void ProjectAlan()
    {
        //gets 2D Alan's direction
        int direction = Alan2D.transform.localScale.x > 0 ? 1 : -1;
        
        //Scales based on distance from InvisaWall
        float distanceToPlane = projectedWallTransform.position.z - transform.position.z;
        float scaleFactor =  2*(1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane))); // Avoid division by zero
        Vector3 theScale = alanDefaultScale * scaleFactor;
        theScale.x *= direction;
        Alan2D.transform.localScale = theScale;
        
        //Moves to corresponding X position
        Vector3 newXPosition = Alan2D.transform.position;
        newXPosition.x = transform.position.x;
        Alan2D.transform.position = newXPosition;
        
        //Moves to corresponding Y position
        Vector3 newYPosition = Alan2D.transform.position;
        newYPosition.y = transform.position.y;
        Alan2D.transform.position = newYPosition;
    }

    private void SetObjectionProjectionInstance()
    {
        currentHeldObjectProjection = GetComponentInChildren<ObjectProjection>();
    }
    
    private void AttachBlockToAlan2D()
    {
        // Determine the direction the character is facing
        float direction = Mathf.Sign(Alan2D.transform.localScale.x);

        // Calculate the new position of the block
        Vector3 newPosition = Alan2D.transform.position + new Vector3(direction * 0.7f, 1f, 0f);
        
        
        currentHeldObjectProjection.PositionBlockToHoldPosition(holdPosition2D.position);
        currentHeldObjectProjection.SetBlockParent(Alan2D.transform);
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
    
}
