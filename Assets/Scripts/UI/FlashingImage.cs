using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashingImage : MonoBehaviour
{
    public Image targetImage;       // Assign your UI Image here
    public Sprite image1;           // First image
    public Sprite image2;           // Second image
    public float switchInterval = 0.5f;

    private Coroutine flashCoroutine;

    private void OnEnable()
    {
        flashCoroutine = StartCoroutine(FlashImage());
    }

    private void OnDisable()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }

    IEnumerator FlashImage()
    {
        while (true)
        {
            targetImage.sprite = image1;
            yield return new WaitForSeconds(switchInterval);
            targetImage.sprite = image2;
            yield return new WaitForSeconds(switchInterval);
        }
    }
}