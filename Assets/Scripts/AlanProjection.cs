using System.Collections;
using UnityEngine;

public class AlanProjection : MonoBehaviour
{
    private Coroutine scaleCoroutine;
    private Vector3 alanDefaultScale;
    private float initialDistanceFromWall;
    
    private Transform projectedWallTransform;

    void Start()
    {
        alanDefaultScale = transform.localScale;
        EventManager.instance.OnInstantiateGamePlay += AttachProjectedWallTransform;
        EventManager.instance.OnLoadScene += AttachProjectedWallTransform;
        EventManager.instance.OnToggleTwoD += ScaleObject;
    }

    private void AttachProjectedWallTransform()
    {
        projectedWallTransform = GameObject.FindWithTag("ProjectedWallTransform").transform;
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
            ScaleObject();

            yield return new WaitForSeconds(0.05f); // Adjust frequency (every 0.05s instead of every frame)
        }
    }

    private void ScaleObject()
    {
        float distanceToPlane = projectedWallTransform.position.z - transform.position.z;
        float computedScaleFactor = initialDistanceFromWall * (1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane)));
        int direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 theScale = alanDefaultScale * computedScaleFactor;
        theScale.x *= direction;
        transform.localScale = theScale;
    }

    // 🚀 **Trigger-based Start & Stop** 🚀
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DynamicScaleBox")) // Make sure it's the correct trigger
        {
            print("Started Scaling");
            StartScaling();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DynamicScaleBox"))
        {
            StopScaling();
        }
    }
}
