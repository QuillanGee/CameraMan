using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSoundsScene : MonoBehaviour
{
    public AudioSource Bark;
    public AudioSource Electricity;
    public AudioSource TocToc;
    public AudioSource Shot;
    public AudioSource Bird;

    // Originally it was not public
    public bool activeAll = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            if(Bark.isPlaying)
                Bark.Stop();
            else
                Bark.Play();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            if (Electricity.isPlaying)
                Electricity.Stop();
            else
                Electricity.Play();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            if (TocToc.isPlaying)
                TocToc.Stop();
            else
                TocToc.Play();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            if (Shot.isPlaying)
                Shot.Stop();
            else
                Shot.Play();

        if (Input.GetKeyDown(KeyCode.Alpha5))
            if (Bird.isPlaying)
                Bird.Stop();
            else
                Bird.Play();

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (Bird.isPlaying && Shot.isPlaying && TocToc.isPlaying && Electricity.isPlaying && Bark.isPlaying)
            {
                Bird.Stop();
                Shot.Stop();
                TocToc.Stop();
                Electricity.Stop();
                Bark.Stop();
            }
            else
            {
                Bird.Play();
                Shot.Play();
                TocToc.Play();
                Electricity.Play();
                Bark.Play();
            }
        }

    }
}
