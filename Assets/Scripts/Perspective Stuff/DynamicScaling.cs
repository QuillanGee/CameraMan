using System.Collections;
using UnityEngine;

public class DynamicScaling : MonoBehaviour
{
    private Coroutine scaleCoroutine;
    private Vector3 alanDefaultScale;
    private float initialDistanceFromWall;
    private Transform wallTransform;
    private float scaleFactor = 7.0f;
    float minScale = 0.5f;  // Example minimum scale
    float maxScale = 5f;  // Example maximum scale

    void Start()
    {
        alanDefaultScale = transform.localScale;
    }

    void StartScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        wallTransform = StaticProjectedWallTransform.ProjectedWallTransform;
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
            ScaleObject();

            yield return new WaitForSeconds(0.05f); // Adjust frequency (every 0.05s instead of every frame)
        }
    }

    private void ScaleObject()
    {
        float distanceToPlane = StaticProjectedWallTransform.ProjectedWallTransform.position.z - transform.position.z;
        float computedScaleFactor =  scaleFactor * (1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane)));
        int direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 theScale = alanDefaultScale * computedScaleFactor;
        theScale.x = Mathf.Clamp(theScale.x, minScale, maxScale);
        theScale.y = Mathf.Clamp(theScale.y, minScale, maxScale);
        theScale.z = Mathf.Clamp(theScale.z, minScale, maxScale);
        theScale.x *= direction;
        transform.localScale = theScale;
    }

    // 🚀 **Trigger-based Start & Stop** 🚀
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DynamicScaleBox")) // Make sure it's the correct trigger
        {
            StartScaling();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DynamicScaleBox"))
        {
            StopScaling();
            ResetScale();
        }
    }

    private void ResetScale()
    {
        transform.localScale = alanDefaultScale;
    }
}
