using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnPostToggleFirstPerson += HideBlur;
        EventManager.instance.OnPostToggleTwoD += ShowBlur;
        HideBlur();
    }

    private void OnDestroy()
    {
        EventManager.instance.OnPostToggleFirstPerson -= HideBlur;
        EventManager.instance.OnPostToggleTwoD -= ShowBlur;
    }

    private void ShowBlur()
    {
        gameObject.SetActive(true);
    }

    private void HideBlur()
    {
        gameObject.SetActive(false);
    }
}
