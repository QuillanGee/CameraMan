using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct Line
    {
        public string speakerID;
        public string line;
        public AudioClip clip;
        public bool subtitleShowsSpeaker;
        public Color colorSpeaker;
        public Color colorText;
        [Range(0.8f, 7)]
        public float time;
        public GameObject speaker;
    }

    public Line[] line;
    public Subtitle sub;
    
    private List<LineSubtitle> lines = new List<LineSubtitle>();
    private int currentLine = -1;  // No dialogue playing at start

    void Start()
    {
        // Convert struct into ScriptableObject instances
        for (int i = 0; i < line.Length; i++)
        {
            LineSubtitle l = ScriptableObject.CreateInstance<LineSubtitle>();
            l.speakerID = line[i].speakerID;
            l.line = line[i].line;
            l.clip = line[i].clip;
            l.subtitleShowsSpeaker = line[i].subtitleShowsSpeaker;
            l.colorSpeaker = line[i].colorSpeaker;
            l.colorText = line[i].colorText;
            l.time = line[i].time;
            l.speaker = line[i].speaker;
            lines.Add(l);
        }
    }

    // 🎯 Public function to trigger a specific line
    public void PlayDialogue(int lineIndex)
    {
        if (lineIndex < 0 || lineIndex >= lines.Count)
        {
            Debug.LogWarning("Invalid dialogue index.");
            return;
        }

        // Remove previous subtitle
        if (currentLine >= 0)
            sub.RemoveLinesActive(lines[currentLine]);

        currentLine = lineIndex;
        sub.PutLinesActive(lines[currentLine]); // Display subtitle
        AudioSource speakerAudio = lines[currentLine].speaker.GetComponent<AudioSource>();

        if (speakerAudio != null)
        {
            speakerAudio.clip = lines[currentLine].clip;
            speakerAudio.Play();
        }
    }
}