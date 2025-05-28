using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable2DPlatform : MonoBehaviour
{
    [SerializeField] private MoveablePlatform sister3DPlatform;
    public Transform[] waypoints;
    public float speed = 3f;
    private int targetIndex = 0;
    private bool isMoving = true;
    private bool isPause = false;

    private void Start()
    {
        // Subscribe to the event from MoveablePlatform (3D)
        if (sister3DPlatform != null)
        {
            sister3DPlatform.OnWaypointReached += HandleWaypointReached;
        }

        EventManager.instance.OnPauseGamePlay += HandlePause;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (sister3DPlatform != null)
        {
            sister3DPlatform.OnWaypointReached -= HandleWaypointReached;
        }
        EventManager.instance.OnPauseGamePlay -= HandlePause;

    }

    private void Update()
    {
        if (isPause || !isMoving || waypoints.Length == 0) return;
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[targetIndex].position) < 0.1f)
        {
            OnReachWaypoint();
        }
    }

    private void OnReachWaypoint()
    {
        targetIndex = (targetIndex + 1) % waypoints.Length;
        isMoving = false;
    }

    private void HandleWaypointReached(bool isParallel)
    {
        isMoving = !isParallel;  // Stop moving if parallel to the camera
    }

    private void HandlePause(object sender, bool pause)
    {
        isPause = pause;
    }
}