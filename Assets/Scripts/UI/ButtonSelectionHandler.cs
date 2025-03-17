using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeepButtonSelected : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject lastSelected;

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        // If no object is selected but we have a last selected one, re-select it
        if (eventSystem.currentSelectedGameObject == null && lastSelected != null)
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }

        // Update last selected object when a new button is clicked
        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject.GetComponent<Button>() != null)
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
    }
}