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
    private float zAxisToProjectAlan;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        crossHair = GetComponentInChildren<Canvas>();
        EventManager.instance.OnToggleFirstPerson += ProjectAlan2DToMoveAlan;
        EventManager.instance.OnToggleTwoD += DisableCrossHair;
        EventManager.instance.OnPostToggleFirstPerson += EnableCrossHair;
        EventManager.instance.OnResetAlan += ResetPosition;
        EventManager.instance.OnHoldingBlock += SetObjectionProjectionInstance;
        EventManager.instance.OnHoldingBlock += AttachBlockToAlan2D;
        EventManager.instance.OnLoadScene += AttachResetPosition;
        // EventManager.instance.OnPostToggleFirstPerson += ShowAlan;
        // EventManager.instance.OnPostToggleTwoD += HideAlan;
        EventManager.instance.OnSendZAxis += SetZAxisToProjectAlan;
    }

    private void AttachResetPosition()
    {
        resetPosition = GameObject.FindWithTag("ResetPosition").transform;
    }
    
    private void ProjectAlan2DToMoveAlan()
    {
        Vector3 newPosition = new Vector3(Alan2D.position.x, Alan2D.position.y, transform.position.z);
        transform.position = newPosition;
    }

    private void SetZAxisToProjectAlan(object sender, ZAxisEventArgs args)
    {
        if (args.IsOnPlatform)
        {
            zAxisToProjectAlan = args.ZAxis;
        }
        else
        {
            zAxisToProjectAlan = transform.position.z;
        }
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
}
