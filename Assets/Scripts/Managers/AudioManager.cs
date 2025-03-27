using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    [SerializeField] private AudioClip glitchSoundEffect;
    [SerializeField] private AudioClip blockPickUp;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlitchSoundEffect()
    {
        sfxSource.PlayOneShot(glitchSoundEffect);
    }

    public void PlayBlockPickUp()
    {
        sfxSource.PlayOneShot(blockPickUp);
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