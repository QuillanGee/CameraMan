using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour
{
    public List<Text> textBox = new List<Text>();
    public List<Text> nameFields = new List<Text>();
    public GameObject subtitleArea;

    private List<LineSubtitle> linesActive = new List<LineSubtitle>();
    private List<LineSubtitle> allLinesActive = new List<LineSubtitle>();
    private int numberActive;
    [Range(25, 40)]
    public int fontSize = 28;
    [Range(20, 45)]
    public int nCharacters = 35;
    [Range(0.2f, 1)]
    public float backgroundAlpha;
    // Start is called before the first frame update
    void Start()
    {
        for(int i= 0; i < textBox.Count; i++)
        {
            textBox[i].fontSize = fontSize;
            nameFields[i].fontSize = fontSize;
        }
        subtitleArea.GetComponent<Image>().color = new Color(0, 0, 0, backgroundAlpha);
        subtitleArea.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    void createSubtitle()
    {
        linesActive.Clear();
        numberActive = 0;
        int nSubtitles = textBox.Count;

        for (int i = 0; i < allLinesActive.Count; i++)
        {
            if (linesActive.Count == textBox.Count)
            {
                LineSubtitle actualLine = allLinesActive[i];
                for (int j = 0; j < linesActive.Count; j++)
                {
                    if (linesActive[j].power < actualLine.power)
                    {
                        actualLine = linesActive[j];
                    }
                }
                if (actualLine != allLinesActive[i])
                {
                    linesActive.Remove(actualLine);
                    linesActive.Add(allLinesActive[i]);
                }
            }
            if (i < allLinesActive.Count)
                linesActive.Add(allLinesActive[i]);
        }

        numberActive = linesActive.Count;
        if (numberActive > 0)
        {
            for (int i = 0; i < nSubtitles; i++)
            {
                if (i < numberActive)
                {
                    textBox[i].enabled = true;
                    textBox[i].text = "";
                    textBox[i].color = linesActive[i].colorText;
                    int nt = 0;
                    for (int j = 0; j < linesActive[i].line.Length; j++)
                    {
                        if (nt >= nCharacters - 1 && linesActive[i].line[j] == ' ')
                        {
                            textBox[i].text += "\n";
                            nt = 0;
                        }
                        else
                        {
                            nt++;
                            textBox[i].text += linesActive[i].line[j];
                        }
                    }
                    //textBox[i].text = "Buenas \n" + linesActive[i].line[0];

                    if (linesActive[i].subtitleShowsSpeaker)
                    {
                        nameFields[i].enabled = true;
                        nameFields[i].text = linesActive[i].speakerID + ": ";
                        nameFields[i].color = linesActive[i].colorSpeaker;
                    }
                    else
                        nameFields[i].enabled = false;
                }
                else
                {
                    textBox[i].enabled = false;
                    nameFields[i].enabled = false;
                }
            }
            subtitleArea.SetActive(true);
        }
        else
        {
            subtitleArea.SetActive(false);
        }
    }

    public void PutLinesActive(LineSubtitle l)
    {        
        allLinesActive.Add(l);
        createSubtitle();
    }

    public void RemoveLinesActive(LineSubtitle l)
    {
        allLinesActive.Remove(l);
        createSubtitle();
    }
}

