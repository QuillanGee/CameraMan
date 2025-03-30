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
    public int power;
    public bool startOnAwake = true;
    public Subtitle sub;
    List<LineSubtitle> lines = new List<LineSubtitle>();
    //LineSubtitle ActualLine = new LineSubtitle();
    private int nLine = 0;
    private int actualL = -1;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < line.Length; i++)
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
            l.power = power;
            lines.Add(l);
        }

        nLine = lines.Count;

        if (startOnAwake)
        {
            //StartCoroutine(ConverseDelay(0.2f)); // Starting with a delay makes sure that every component has initiated.
            StartConverse(0.2f);
        }
    }

    public void StartConverse(float startTime)
    {
        StartCoroutine(ConverseDelay(startTime));
    }

    IEnumerator ConverseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(actualL >= 0)
            sub.RemoveLinesActive(lines[actualL]);
        SpeakSubtitle();
    }

    void SpeakSubtitle()
    {
        actualL++;

        if (actualL < nLine)
        {
            sub.PutLinesActive(lines[actualL]);
            lines[actualL].speaker.GetComponent<AudioSource>().clip = lines[actualL].clip;
            lines[actualL].speaker.GetComponent<AudioSource>().Play();
            if (actualL <= lines.Count)
                StartCoroutine(ConverseDelay(lines[actualL].time));
        }
    }
}
