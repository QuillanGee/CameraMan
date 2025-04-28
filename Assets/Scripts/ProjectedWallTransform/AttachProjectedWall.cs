using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachProjectedWall : MonoBehaviour
{
    [SerializeField] private Transform projectedWall;
    [SerializeField] private Transform zAxis;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StaticProjectedWallTransform.Instance.SetWall(projectedWall);
            StaticZAxisFor2DLevel.Instance.SetZAxis(zAxis);
        }
    }
}
