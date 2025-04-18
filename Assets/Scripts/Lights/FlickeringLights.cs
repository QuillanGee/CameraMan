using UnityEngine;

public class BrokenFlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.1f;  // Minimum light intensity
    public float maxIntensity = 1f;    // Maximum light intensity
    public float flickerSpeed = 0.5f;  // Speed of flicker transition (how fast the intensity changes)
    public float flickerInterval = 1f; // Time between flickers, light stays on for a longer period before turning off
    
    private Light lightSource;
    private bool isLightOn;
    private float timer;

    void Start()
    {
        lightSource = GetComponent<Light>();
        isLightOn = true;  // Start with the light on
        lightSource.intensity = Random.Range(minIntensity, maxIntensity); // Set initial random intensity
        timer = flickerInterval; // Set initial timer to the flicker interval
    }

    void Update()
    {
        timer -= Time.deltaTime;  // Decrease the timer by the frame time

        // When the timer reaches zero, switch the light state
        if (timer <= 0f)
        {
            isLightOn = !isLightOn;
            timer = flickerInterval; // Reset the timer to the flicker interval

            if (isLightOn)
            {
                // Light is on, set intensity randomly within the range
                lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            }
            else
            {
                // Light is off, set intensity to 0 (turn it off)
                lightSource.intensity = 0f;
            }
        }

        // Smoothly transition the light intensity to the target intensity
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, lightSource.intensity, Time.deltaTime * flickerSpeed);
    }
}