using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource dialogueSource;
    [SerializeField] private AudioClip glitchSoundEffect;
    [SerializeField] private AudioClip blockPickUp;
    [SerializeField] private AudioClip whiteboard;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        dialogueSource.PlayOneShot(clip);
    }

    public void PlayInteractionSound(string tagName)
    {
        if (tagName == "whiteboard")
        {
            sfxSource.PlayOneShot(whiteboard);
        }

        if (tagName == "block")
        {
            sfxSource.PlayOneShot(blockPickUp);
        }
    }

    public void PlayGlitchSoundEffect()
    {
        sfxSource.PlayOneShot(glitchSoundEffect);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}