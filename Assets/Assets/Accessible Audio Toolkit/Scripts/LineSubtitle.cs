using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSubtitle : ScriptableObject
{
    public string speakerID;
    public string line;
    public AudioClip clip;
    public bool subtitleShowsSpeaker;
    public Color colorSpeaker;
    public Color colorText;
    public float time;
    public int power;
    public GameObject speaker;
}
