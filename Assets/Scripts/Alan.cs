using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan : MonoBehaviour
{
    [SerializeField] Transform Alan2D;
    [SerializeField] private Transform holdPosition2D;
    private Canvas crossHair;
    private Vector3 startingPosition;
    private ObjectProjection currentHeldObjectProjection;
    [SerializeField] private Transform AlanMesh;
    [SerializeField] private Transform CapsuleHolder;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        crossHair = GetComponentInChildren<Canvas>();
        startingPosition = transform.position;
        
        EventManager.instance.OnToggleFirstPerson += ProjectAlan2DToMoveAlan;
        EventManager.instance.OnToggleTwoD += DisableCrossHair;
        EventManager.instance.OnPostToggleFirstPerson += EnableCrossHair;
        EventManager.instance.OnPostToggleFirstPerson += ParentMeshToAlan;
        EventManager.instance.OnPostToggleTwoD += ParentMeshToAlan2D;
        EventManager.instance.OnResetAlan += ResetPosition;
        EventManager.instance.OnHoldingBlock += SetObjectionProjectionInstance;
        EventManager.instance.OnHoldingBlock += AttachBlockToAlan2D;
    }
    

    private void ProjectAlan2DToMoveAlan()
    {
        //Moves to corresponding X position
        Vector3 newPositionX = transform.position;
        newPositionX.x = Alan2D.position.x;
        transform.position = newPositionX;
        
        //Moves to corresponding X position
        Vector3 newPositionY = transform.position;
        newPositionY.y = Alan2D.position.y;
        transform.position = newPositionY;
    }
    
    private void DisableCrossHair()
    {
        crossHair.enabled = false;
    }

    private void EnableCrossHair()
    {
        crossHair.enabled = true;
    }
    
    private void SetObjectionProjectionInstance()
    {
        currentHeldObjectProjection = GetComponentInChildren<ObjectProjection>();
    }
    
    private void AttachBlockToAlan2D()
    {
        currentHeldObjectProjection.PositionBlockToHoldPosition(holdPosition2D.position);
        currentHeldObjectProjection.SetBlockParent(Alan2D);
    }

    private void ResetPosition()
    {
        transform.position = startingPosition;
    }

    private void ParentMeshToAlan()
    {
        //set the parent of mesh back to this 
        AlanMesh.SetParent(CapsuleHolder);
    }
    private void ParentMeshToAlan2D()
    {
        //set the parent of mesh back to this 
        AlanMesh.SetParent(Alan2D);
    }
}
