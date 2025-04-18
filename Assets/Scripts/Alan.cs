using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan : MonoBehaviour
{
    [SerializeField] Transform Alan2D;
    [SerializeField] private Transform holdPosition2D;
    private Canvas crossHair;
    [SerializeField] Transform resetPosition;
    private ObjectProjection currentHeldObjectProjection;
    [SerializeField] private GameObject AlanMesh;
    [SerializeField] private Transform CapsuleHolder;
    private float zAxisToProjectAlan = -1f;
    
    private bool isHoldingBlock = false;
    private bool isOnObject = false;
    
    private Rigidbody rb;

    [SerializeField] private bool isCrosshairEnabled = true;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        crossHair = GetComponentInChildren<Canvas>();
        rb = GetComponent<Rigidbody>();
        EventManager.instance.OnPostToggleTwoD += ToggleTwoDAction;
        EventManager.instance.OnPostToggleFirstPerson += ToggleFirstPersonAction;
        EventManager.instance.OnResetAlan += ResetPosition;
        EventManager.instance.OnLoadScene += AttachResetPosition;
        EventManager.instance.OnSendZAxis += SetZAxisToProjectAlan;
        EventManager.instance.OnHoldingBlock += SetHoldingBlockTrue;
        EventManager.instance.OnNotHoldingBlock += SetHoldingBlockFalse;
    }

    private void AttachResetPosition()
    {
        resetPosition = GameObject.FindWithTag("ResetPosition").transform;
    }

    private void ToggleFirstPersonAction()
    {
        EnableCrossHair();
        ProjectAlan2DToMoveAlan();
        ShowAlan();
    }

    private void ToggleTwoDAction()
    {
        DisableCrossHair();
        HideAlan();
        if (isHoldingBlock)
        {
            SetObjectionProjectionInstance();
            AttachBlockToAlan2D();
        }
    }
    
    private void ProjectAlan2DToMoveAlan()
    {
        if (!isOnObject)
        {
            zAxisToProjectAlan = transform.position.z;
        }
        isOnObject = false;
        Vector3 newPosition = new Vector3(Alan2D.position.x, Alan2D.position.y, zAxisToProjectAlan);
        rb.MovePosition(newPosition);
    }

    private void SetZAxisToProjectAlan(object sender, float zAxis)
    {
        isOnObject = true;
        zAxisToProjectAlan = zAxis;
    }
    
    private void DisableCrossHair()
    {
        if (isCrosshairEnabled)
        {
            crossHair.enabled = false;
        }
    }

    private void EnableCrossHair()
    {
        if (isCrosshairEnabled)
        {
            crossHair.enabled = true;
        }
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
        transform.position = resetPosition.position;
    }
    
    private void HideAlan()
    {
        if (AlanMesh.activeInHierarchy)
        {
            AlanMesh.SetActive(false);
        }
    }

    private void ShowAlan()
    {
        if (!AlanMesh.activeInHierarchy)
        {
            AlanMesh.SetActive(true);
        }
    }

    private void SetHoldingBlockTrue()
    {
        isHoldingBlock = true;
    }

    private void SetHoldingBlockFalse()
    {
        isHoldingBlock = false;
    }
}
