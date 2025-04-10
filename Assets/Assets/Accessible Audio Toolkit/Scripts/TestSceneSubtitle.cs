using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneSubtitle : MonoBehaviour
{
    public EventsSubtitles subt;
    public GameObject gObject;
    public GameObject gObject1;
    public GameObject gObject2;

    bool gObjActive = false;
    bool gObjActive1 = false;
    bool gObjActive2 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !gObjActive)
        {
            subt.PutEventSubtitle(1, "[RADIO]", 1, gObject);
            gObject.GetComponent<AudioSource>().Play();
            gObject.GetComponentInChildren<TextMesh>().color = Color.blue;
            gObjActive = true;
            Invoke("RemovegameObject", 21.0f);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !gObjActive1)
        {
            subt.PutEventSubtitle(2, "[PHONE]", 1, gObject1);
            gObject1.GetComponent<AudioSource>().Play();
            gObject1.GetComponentInChildren<TextMesh>().color = Color.magenta;
            gObjActive1 = true;
            Invoke("RemovegameObject2", 8.5f);
        }

        if (Input.GetKeyDown(KeyCode.R) && !gObjActive2)
        {
            subt.PutEventSubtitle(3, "[VOICE]", 2, gObject2);
            gObject2.GetComponent<AudioSource>().Play();
            gObject2.GetComponentInChildren<TextMesh>().color = Color.green;
            gObjActive2 = true;
            Invoke("RemovegameObject3", 2f);
        }
    }

    void RemovegameObject()
    {
        gObjActive = false;
        subt.RemoveEventSubtitle(1);
        gObject.GetComponentInChildren<TextMesh>().color = Color.black;
    }

    void RemovegameObject2()
    {
        gObjActive1 = false;
        subt.RemoveEventSubtitle(2);
        gObject1.GetComponentInChildren<TextMesh>().color = Color.black;
    }

    void RemovegameObject3()
    {
        gObjActive2 = false;
        subt.RemoveEventSubtitle(3);
        gObject2.GetComponentInChildren<TextMesh>().color = Color.black;
    }
}
