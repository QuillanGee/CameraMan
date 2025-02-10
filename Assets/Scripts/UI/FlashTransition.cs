using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FlashTransition : MonoBehaviour
{
    public CanvasGroup flashPanel;
    public float flashDuration = 0.2f;


    void Start()
    {
        flashPanel.alpha = 0; // Make sure it's invisible at the start
    }


    public void Flash()
    {
        StartCoroutine(FlashEffect());
    }


    IEnumerator FlashEffect()
    {
        flashPanel.alpha = 1;
        yield return new WaitForSeconds(flashDuration);
        flashPanel.alpha = 0;
    }
}