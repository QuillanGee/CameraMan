using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EventsSubtitles : MonoBehaviour
{

    [System.Serializable]
    public struct lineSubtitle
    {
        public GameObject arealineSubtitle;
        public GameObject textSubtitle;
        public GameObject radarSubtitle;
    }
    public struct objectSound
    {
        public string text;
        public int power;
        public GameObject transmitter;
    }

    private Dictionary<int, objectSound> closedCaptions = new Dictionary<int, objectSound>();
    private Dictionary<int, objectSound> openCaptions = new Dictionary<int, objectSound>();

    private int[] power;
    private int[] vectorID;

    public lineSubtitle[] textSubtitle;

    public GameObject areaSubtitle;

    public GameObject Player;

    void Start()
    {
        power = new int[textSubtitle.Length];
        vectorID = new int[textSubtitle.Length];
        for (int i = 0; i < textSubtitle.Length; i++)
        {
            textSubtitle[i].arealineSubtitle.SetActive(false);
            power[i] = -1;
            vectorID[i] = -1;
        }
        areaSubtitle.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 n = new Vector3(0, 0, 0);
        n.z = Player.transform.eulerAngles.y;
        for (int i = 0; i < textSubtitle.Length; i++)
        {
            if ((textSubtitle[i].arealineSubtitle.activeSelf))
            {                
                Quaternion radarQuaternion;
                Vector3 dir = openCaptions[vectorID[i]].transmitter.transform.position - Player.transform.position;
                radarQuaternion = Quaternion.LookRotation(dir);

                radarQuaternion.z = -radarQuaternion.y;
                radarQuaternion.x = 0;
                radarQuaternion.y = 0;

                textSubtitle[i].radarSubtitle.transform.localRotation = radarQuaternion * Quaternion.Euler(n);
            }
        }
    }

    public void PutEventSubtitle(int id, string text, int force, GameObject g)
    {
        if (!openCaptions.ContainsKey(id) && !closedCaptions.ContainsKey(id))
        {
            areaSubtitle.SetActive(true);
            if (openCaptions.Count == textSubtitle.Length)
            {
                int idMin = -1;
                int index = -1;
                for (int i = 0; i < vectorID.Length; i++)
                {
                    if (vectorID[i] < id && (vectorID[i] < idMin || idMin == -1))
                    {
                        idMin = vectorID[i];
                        index = i;
                    }
                }
                if (index != -1)
                {
                    AddClosedCaptions(idMin, openCaptions[idMin].text, openCaptions[idMin].power, openCaptions[idMin].transmitter);
                    openCaptions.Remove(idMin);

                    AddOpenCaptions(id, text, force, g);

                    textSubtitle[index].textSubtitle.GetComponent<Text>().text = text;
                    power[index] = force;
                    vectorID[index] = id;
                    return;
                }
            }
            else
            {
                for (int i = 0; i < textSubtitle.Length; i++)
                {
                    if (!textSubtitle[i].arealineSubtitle.activeSelf)
                    {
                        AddOpenCaptions(id, text, force, g);

                        textSubtitle[i].arealineSubtitle.SetActive(true);
                        textSubtitle[i].textSubtitle.GetComponent<Text>().text = text;
                        power[i] = force;
                        vectorID[i] = id;
                        return;
                    }
                }
            }

            AddClosedCaptions(id, text, force, g);
        }
        else
            Debug.LogError("The indicated id does exist");
    }

    void AddClosedCaptions(int id, string text, int force, GameObject g)
    {
        objectSound o;
        o.text = text;
        o.power = force;
        o.transmitter = g;
        closedCaptions.Add(id, o);
    }

    void AddOpenCaptions(int id, string text, int force, GameObject g)
    {
        objectSound ob;
        ob.text = text;
        ob.power = force;
        ob.transmitter = g;
        openCaptions.Add(id, ob);
    }

    public void RemoveEventSubtitle(int id)
    {
        if (openCaptions.ContainsKey(id))
        {
            openCaptions.Remove(id);
            int i = 0;
            bool search = false;
            do
            {
                if (vectorID[i] == id)
                {

                    textSubtitle[i].textSubtitle.GetComponent<Text>().text = "";
                    power[i] = -1;
                    vectorID[i] = -1;
                    textSubtitle[i].arealineSubtitle.SetActive(false);

                    if (openCaptions.Count == 0)
                        areaSubtitle.SetActive(false);

                    search = true;
                }

                i++;

            } while (!search && i < vectorID.Length);

            if (closedCaptions.Count > 0)
            {
                int p = -1;
                int identificator = 0;
                foreach (KeyValuePair<int, objectSound> result in closedCaptions)
                {
                    if (result.Value.power > p)
                    {
                        identificator = result.Key;
                    }
                }

                objectSound newCaptions = closedCaptions[identificator];
                closedCaptions.Remove(identificator);

                PutEventSubtitle(identificator, newCaptions.text, newCaptions.power, newCaptions.transmitter);

            }
        }

        else if (closedCaptions.ContainsKey(id))
            closedCaptions.Remove(id);

        else
            Debug.LogError("The indicated id does not exist");
    }
}

