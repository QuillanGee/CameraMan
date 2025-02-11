using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class FlashTransition : MonoBehaviour
{
    public CanvasGroup flashPanel;
    public float flashDuration = 1f;
    public AudioSource audioSource;


    void Start()
    {
        flashPanel.alpha = 0; // Make sure it's invisible at the start
    }


    public void Flash()
    {
        audioSource.Play();
        StartCoroutine(FlashEffect());
    }


    IEnumerator FlashEffect()
    {
        flashPanel.alpha = 1;
        yield return new WaitForSeconds(flashDuration);
        SceneManager.LoadScene("Level 4");
        flashPanel.alpha = 0;
    }
}