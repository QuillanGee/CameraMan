using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUI : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isVisibleOnStart = true;
    void Start()
    {
        EventManager.instance.OnPostToggleFirstPerson += HideCameraUI;
        EventManager.instance.OnPostToggleTwoD += ShowCameraUI;
        gameObject.SetActive(isVisibleOnStart);
    }

    private void OnDestroy()
    {
        EventManager.instance.OnPostToggleFirstPerson -= HideCameraUI;
        EventManager.instance.OnPostToggleTwoD -= ShowCameraUI;
    }

    // Update is called once per frame
    private void ShowCameraUI()
    {
        gameObject.SetActive(true);
    }
    
    private void HideCameraUI()
    {
        gameObject.SetActive(false);
    }
}
