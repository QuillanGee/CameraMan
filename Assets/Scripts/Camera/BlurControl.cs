using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlurControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material blurMaterial; // Assign the shader material in the inspector

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (blurMaterial != null)
        {
            Graphics.Blit(source, destination, blurMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
    void Start()
    {
        // EventManager.instance.OnPostToggleFirstPerson += HideBlur;
        // EventManager.instance.OnPostToggleTwoD += ShowBlur;
    }

    private void OnDestroy()
    {
        // EventManager.instance.OnPostToggleFirstPerson -= HideBlur;
        // EventManager.instance.OnPostToggleTwoD -= ShowBlur;
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
