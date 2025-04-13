using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component if it doesn't exist already
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure the audio source
        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;
    }

    void Update()
    {
        // Check for mouse click (left button)
        if (Input.GetMouseButtonDown(0))
        {
            PlayClickSound();
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Click sound or audio source not assigned!");
        }
    }
}