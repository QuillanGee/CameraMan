using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
    [System.Serializable]
    public struct WayPointData
    {
        public Transform waypoint;
        public bool isParallelToCameraMovement;
    }

    public WayPointData[] waypoints;
    public float speed = 3f;
    private int targetIndex = 0;
    private bool isPaused = false;

    public delegate void WaypointReachedEvent(bool isParallel);
    public event WaypointReachedEvent OnWaypointReached;


    private void Start()
    {
        EventManager.instance.OnPauseGamePlay += HandlePause;
    }

    private void OnDestroy()
    {
        EventManager.instance.OnPauseGamePlay -= HandlePause;
    }
    protected virtual void Update()
    {
        if (isPaused || waypoints.Length == 0) return;
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].waypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[targetIndex].waypoint.position) < 0.1f)
        {
            OnReachWaypoint();
        }
    }

    private void OnReachWaypoint()
    {
        targetIndex = (targetIndex + 1) % waypoints.Length;
        bool isParallel = waypoints[targetIndex].isParallelToCameraMovement;
        
        // Notify listeners (2D platform) about the waypoint change
        OnWaypointReached?.Invoke(isParallel);
    }

    private void HandlePause(object sender, bool pause)
    {
        isPaused = pause;
    }
}