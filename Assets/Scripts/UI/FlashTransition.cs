using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FlashTransition : MonoBehaviour
{
    public CanvasGroup flashPanel;
    public float flashDuration = 0.2f;
    public AudioSource audioSource;


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
        audioSource.Play();
        yield return new WaitForSeconds(flashDuration);
        SceneManager.LoadScene("Level Demo Modified");
        yield return new WaitForSeconds(0.2f);
        flashPanel.alpha = 0;
    }
}