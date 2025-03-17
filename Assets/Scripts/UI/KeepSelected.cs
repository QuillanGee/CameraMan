using UnityEngine;
using UnityEngine.EventSystems;

public class KeepSelected : MonoBehaviour
{
    private GameObject lastSelected;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Restore last selected button when clicking outside
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
        else
        {
            // Update last selected button if a new UI element is clicked
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
}