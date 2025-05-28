using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour
{
    public List<GameObject> panels; // Drag your UI panels here in the inspector

    // Function to activate a specific panel and disable others
    public void ShowPanel(GameObject selectedPanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == selectedPanel);
        }
    }
}
