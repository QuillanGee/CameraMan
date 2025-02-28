using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisibilityManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnHideLevel += HideLevel;
        EventManager.instance.OnShowLevel += ShowLevel;
    }

    private void HideLevel()
    {
        gameObject.SetActive(false);
    }

    private void ShowLevel()
    {
        gameObject.SetActive(true);
    }
}
