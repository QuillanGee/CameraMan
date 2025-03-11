using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisibilityManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnPostToggleFirstPerson += PostToggleFirstPerson;
        EventManager.instance.OnPostToggleTwoD += PostToggleTwoD;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.instance.OnPostToggleFirstPerson -= PostToggleFirstPerson;
        EventManager.instance.OnPostToggleTwoD -= PostToggleTwoD;
    }

    private void PostToggleFirstPerson()
    {
        gameObject.SetActive(false);
    }

    private void PostToggleTwoD()
    {
        gameObject.SetActive(true);
    }
}
