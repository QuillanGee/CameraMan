using UnityEngine;

public class DisableSwitch : MonoBehaviour
{
    private bool isInside = false;

    void Update()
    {
        // Prevent F key press while inside the collider
        if (isInside && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key is disabled in this area!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
        }
    }
}