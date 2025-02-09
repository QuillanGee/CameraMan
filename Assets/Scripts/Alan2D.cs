using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan2D : MonoBehaviour
{
    [SerializeField] Transform projectedWallTransform;
    [SerializeField] GameObject Alan;
    [SerializeField] private Transform holdPosition2D;
    private Vector3 alanDefaultScale;
    private Vector3 startingPosition;
    private ObjectProjection currentHeldObjectProjection;
    private void Start()
    {
        alanDefaultScale = transform.localScale;
        startingPosition = transform.position;
        
        EventManager.instance.OnToggleTwoD += ProjectAlanToMoveAlan2D;
        EventManager.instance.OnHoldingBlock += SetObjectionProjectionInstance;
        EventManager.instance.OnHoldingBlock += AttachBlockToAlan2D;
        EventManager.instance.OnResetAlan2D += ResetPosition;
    }
    
    public void ProjectAlanToMoveAlan2D()
    {
        //gets 2D Alan's direction
        int direction = transform.localScale.x > 0 ? 1 : -1;
        
        //Scales based on distance from InvisaWall
        float distanceToPlane = projectedWallTransform.position.z - Alan.transform.position.z;
        float scaleFactor =  2*(1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane))); // Avoid division by zero
        Vector3 theScale = alanDefaultScale * scaleFactor;
        theScale.x *= direction;
        transform.localScale = theScale;
        
        //Moves to corresponding X position
        Vector3 newXPosition = transform.position;
        newXPosition.x = Alan.transform.position.x;
        transform.position = newXPosition;
        
        //Moves to corresponding Y position
        Vector3 newYPosition = transform.position;
        newYPosition.y = Alan.transform.position.y;
        transform.position = newYPosition;
    }

    private void SetObjectionProjectionInstance()
    {
        currentHeldObjectProjection = GetComponentInChildren<ObjectProjection>();
    }
    
    private void AttachBlockToAlan2D()
    {
        // Determine the direction the character is facing
        float direction = Mathf.Sign(transform.localScale.x);

        // Calculate the new position of the block
        Vector3 newPosition = transform.position + new Vector3(direction * 0.7f, 1f, 0f);
        
        
        currentHeldObjectProjection.PositionBlockToHoldPosition(holdPosition2D.position);
        currentHeldObjectProjection.SetBlockParent(transform);
    }

    private void ResetPosition()
    {
        transform.position = startingPosition;
    }
}
