using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLights : MonoBehaviour
{
    [Header("Flicker Timing")]
    public float minFlickerDelay = 0.05f;  // Minimum time between flickers
    public float maxFlickerDelay = 0.5f;   // Maximum time between flickers

    [Header("Light Intensity")]
    public float minIntensity = 0.3f;      // Minimum light intensity
    public float maxIntensity = 1.5f;      // Maximum light intensity

    [Header("Flicker Chance")]
    [Range(0f, 1f)]
    public float flickerOffChance = 0.2f;  // Chance light turns off completely during flicker

    private Light flickerLight;
    private float originalIntensity;

    void Start()
    {
        flickerLight = GetComponent<Light>();
        originalIntensity = flickerLight.intensity;
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            float delay = Random.Range(minFlickerDelay, maxFlickerDelay);

            // Decide if the light turns off or just dims
            if (Random.value < flickerOffChance)
            {
                flickerLight.enabled = false;
            }
            else
            {
                flickerLight.enabled = true;
                flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
            }

            yield return new WaitForSeconds(delay);
        }
    }
}