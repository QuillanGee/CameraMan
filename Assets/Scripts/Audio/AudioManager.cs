using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
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
        lines.AddRange(line);
        subtitlePanel.SetActive(false); // Hide subtitle UI at start
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

    public void PlayDialogue(List<int> lineIndices)
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

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
