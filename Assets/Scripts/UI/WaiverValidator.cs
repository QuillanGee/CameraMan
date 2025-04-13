using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WaiverValidator : MonoBehaviour
{
    public Toggle[] waiverCheckBoxes;
    public ToggleGroup[] yesNoToggleGroups;
    public Button signatureButton;
    public Button submitButton;
    public Text signText;

    private bool isSigned = false;
    private Coroutine alertCoroutine;

    void Start()
    {
        submitButton.interactable = false;

        foreach (var toggle in waiverCheckBoxes)
            toggle.onValueChanged.AddListener(delegate { AutoEnableSubmit(); });

        foreach (var group in yesNoToggleGroups)
        {
            foreach (var toggle in group.GetComponentsInChildren<Toggle>())
                toggle.onValueChanged.AddListener(delegate { AutoEnableSubmit(); });
        }

        signatureButton.onClick.AddListener(OnSignClicked);
    }

    private void OnSignClicked()
    {
        isSigned = true;
        signatureButton.interactable = false;
        signText.gameObject.SetActive(true);
        AutoEnableSubmit();
    }

    private bool AreCheckBoxesComplete()
    {
        foreach (var checkbox in waiverCheckBoxes)
        {
            if (!checkbox.isOn)
                return false;
        }
        return true;
    }

    private bool AreYesNoGroupsComplete()
    {
        foreach (var group in yesNoToggleGroups)
        {
            if (!group.AnyTogglesOn())
                return false;
        }
        return true;
    }

    private void AutoEnableSubmit()
    {
        submitButton.interactable = AreCheckBoxesComplete() && AreYesNoGroupsComplete() && isSigned;
    }
    
    
}
