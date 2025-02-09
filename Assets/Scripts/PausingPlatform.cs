using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausingPlatform : MoveablePlatform
{
    public float pauseTime = 2f; // Pause duration

    protected override void OnReachWaypoint()
    {
        isMoving = false;
        StartCoroutine(ResumeAfterPause());
    }

    private IEnumerator ResumeAfterPause()
    {
        yield return new WaitForSeconds(pauseTime);
        isMoving = true;
        targetIndex = (targetIndex + 1) % waypoints.Length;
    }
}
