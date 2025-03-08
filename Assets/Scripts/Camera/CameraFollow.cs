using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform alanTransform;
    [SerializeField] private Transform leftMax;
    [SerializeField] private Transform rightMax;
    [SerializeField] private Transform minBottom; // Minimum bottom limit
    [SerializeField] private Transform maxTop; // Maximum top limit
    public float smoothSpeed = 0.125f;  // Speed of the camera smoothing
    private Vector3 offset;

    void Start()
    {
        offset = alanTransform.position;
    }

    void LateUpdate()
    {
        if (alanTransform == null)
            return;

        // Horizontal movement (clamp within left and right max bounds)
        float targetX = Mathf.Clamp(alanTransform.position.x, rightMax.position.x, leftMax.position.x);

        // Vertical movement (clamp within min and max top bounds)
        float targetY = Mathf.Clamp(alanTransform.position.y, minBottom.position.y, maxTop.position.y);

        // Set the desired position
        Vector3 desiredPosition = new Vector3(targetX, targetY, transform.position.z);

        // Smoothly interpolate the camera's position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the new position
        transform.position = smoothedPosition;
    }
}