using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform alanTransform;
    [SerializeField] private Transform leftMax;
    [SerializeField] private Transform rightMax;
    public float smoothSpeed = 0.125f;  // Speed of the camera smoothing
    private Vector3 offset;

    void Start()
    {
        // offset = transform.position - alanTransform.position;
        offset = alanTransform.position;
    }

    void LateUpdate()
    {
        if (alanTransform == null)
            return;

        float targetX = Mathf.Clamp(alanTransform.position.x, rightMax.position.x, leftMax.position.x);
        Vector3 desiredPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}