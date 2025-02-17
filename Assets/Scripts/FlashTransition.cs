using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class FlashTransition : MonoBehaviour
{
    public CanvasGroup flashPanel;
    public float flashDuration = 0.2f;
    public AudioSource flashSound;


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
        flashSound.Play();
        yield return new WaitForSeconds(flashDuration);
        SceneManager.LoadScene("Level 1");
        yield return new WaitForSeconds(0.2f);
        flashPanel.alpha = 0;
    }
}