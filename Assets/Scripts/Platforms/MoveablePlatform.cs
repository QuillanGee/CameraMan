using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
    public Transform[] waypoints; // Points the platform moves between
    public float speed = 3f;      // Movement speed
    protected int targetIndex = 0;  // Index of the next waypoint
    protected bool isMoving = true; // Control movement

    protected virtual void Update()
    {
        if (!isMoving || waypoints.Length == 0) return;

        MovePlatform();
    }

    protected virtual void MovePlatform()
    {
        // Move platform towards the target waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position, speed * Time.deltaTime);

        // Check if the platform has reached the target waypoint
        if (Vector3.Distance(transform.position, waypoints[targetIndex].position) < 0.1f)
        {
            OnReachWaypoint();
        }
    }

    protected virtual void OnReachWaypoint()
    {
        targetIndex = (targetIndex + 1) % waypoints.Length;
    }
}
