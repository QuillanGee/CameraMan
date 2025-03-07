using System.Collections;
using UnityEngine;

public class DynamicScaling : MonoBehaviour
{
    private Coroutine scaleCoroutine;
    private Vector3 alanDefaultScale;
    private float initialDistanceFromWall;
    
    [SerializeField] Transform projectedWallTransform;
    [SerializeField] Collider triggerCollider; // Assign in inspector

    void Start()
    {
        alanDefaultScale = transform.localScale;
        initialDistanceFromWall = Mathf.Abs(transform.position.z - projectedWallTransform.position.z);
    }

    void StartScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleOverTime());
    }

    void StopScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null; // Ensure it's null to prevent multiple calls
        }
    }

    IEnumerator ScaleOverTime()
    {
        while (true) // Loop continuously
        {
            float distanceToPlane = projectedWallTransform.position.z - transform.position.z;
            float computedScaleFactor = initialDistanceFromWall * (1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane)));

            ScaleObject(computedScaleFactor);

            yield return new WaitForSeconds(0.05f); // Adjust frequency (every 0.05s instead of every frame)
        }
    }

    private void ScaleObject(float scaleFactor)
    {
        int direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 theScale = alanDefaultScale * scaleFactor;
        theScale.x *= direction;
        transform.localScale = theScale;
    }

    // 🚀 **Trigger-based Start & Stop** 🚀
    private void OnTriggerEnter(Collider other)
    {
        if (other == triggerCollider) // Make sure it's the correct trigger
        {
            print("Started Scaling");
            StartScaling();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == triggerCollider)
        {
            StopScaling();
        }
    }
}
