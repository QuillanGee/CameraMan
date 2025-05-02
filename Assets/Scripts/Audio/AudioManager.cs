using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource whiteNoise;
    public AudioSource sfxSource;
    public AudioSource dialogueSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip glitchSoundEffect;
    [SerializeField] private AudioClip blockPickUp;
    [SerializeField] private AudioClip whiteboard;

    [Header("Subtitle UI")]
    public GameObject subtitlePanel;
    public TMP_Text subtitleText;

    private Queue<List<int>> dialogueQueue = new Queue<List<int>>();
    private bool isDialoguePlaying = false;
    [SerializeField] private bool isMusicOn = true;
    [SerializeField] private bool isDialogueOn = true;
    

    [System.Serializable]
    public struct Line
    {
        public string line;
        public AudioClip clip;
        public Color colorText;
        [Range(0.8f, 15)]
        public float time;
    }

    public Line[] line;
    private int currentLine = -1;
    private List<Line> lines = new List<Line>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        EventManager.instance.OnTriggerLoadingScene += DestorySelf;
        EventManager.instance.OnPauseGamePlay += HandleSoundsOnPause;
        lines.AddRange(line);
        subtitlePanel.SetActive(false); // Hide subtitle UI at start
        if (!isMusicOn)
        {
            DisableMusic();
        }
    }

    private void DestorySelf()
    {
        Destroy(gameObject);
    }

    private void HandleSoundsOnPause(object sender, bool isPaused)
    {
        if (isPaused)
        {
            whiteNoise.Pause();
            musicSource.Pause();
            dialogueSource.Pause();
            sfxSource.Pause();
        }
        else
        {
            whiteNoise.UnPause();
            musicSource.UnPause();
            dialogueSource.UnPause();
            sfxSource.UnPause();
        }
    }

    private void DisableMusic()
    {
        musicSource.enabled = false;
    }

    public void PlayInteractionSound(string tagName)
    {
        // if (tagName == "whiteboard")
        // {
        sfxSource.PlayOneShot(blockPickUp);
        // }
        //
        // if (tagName == "block")
        // {
        //     sfxSource.PlayOneShot(blockPickUp);
        // }
    }

    public void PlayGlitchSoundEffect()
    {
        sfxSource.PlayOneShot(glitchSoundEffect);
    }

    public void PlayDialogue(List<int> lineIndices)
    {
        if (isDialogueOn)
        {
            if (lineIndices == null || lineIndices.Count == 0)
            {
                Debug.LogWarning("No dialogue indices provided.");
                return;
            }

            dialogueQueue.Enqueue(lineIndices);

            if (!isDialoguePlaying)
            {
                StartCoroutine(ProcessDialogueQueue());
            }
        }
    }

    private IEnumerator ProcessDialogueQueue()
    {
        isDialoguePlaying = true;

        while (dialogueQueue.Count > 0)
        {
            List<int> lineIndices = dialogueQueue.Dequeue();

            for (int i = 0; i < lineIndices.Count; i++)
            {
                int index = lineIndices[i];

                if (index < 0 || index >= lines.Count)
                {
                    Debug.LogWarning($"Invalid dialogue index: {index}");
                    continue;
                }

                Line current = lines[index];

                // Show subtitle UI
                subtitlePanel.SetActive(true);
                subtitleText.text = current.line;

                // Force text to be visible
                Color txtColor = current.colorText;
                txtColor.a = 1f;
                subtitleText.color = txtColor;

                subtitleText.enabled = true;

                // Only play audio on the first line
                if (i == 0 && current.clip != null)
                {
                    dialogueSource.clip = current.clip;
                    dialogueSource.Play();
                }

                yield return new WaitForSeconds(current.time);
            }

            // Hide subtitle UI
            subtitlePanel.SetActive(false);
        }

        isDialoguePlaying = false;
    }

    
    public void FadeMusicVolume(float targetVolume, float duration)
    {
        StartCoroutine(FadeVolumeCoroutine(targetVolume, duration));
    }

    private IEnumerator FadeVolumeCoroutine(float targetVolume, float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        musicSource.volume = targetVolume;
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