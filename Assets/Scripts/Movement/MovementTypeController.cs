using System.Collections;
using UnityEngine;

public class MovementTypeController : MonoBehaviour
{
    [SerializeField] private FirstPersonCharacterMovement firstPersonCharacterMovement;
    [SerializeField] private TwoDCharacterMovement twoDCharacterMovement;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private AudioSource invalidSwitchSound;
    [SerializeField] private GameObject invalidSwitchMessage;

    private bool isInBlurredSpace = false;

    void Start()
    {
        EventManager.instance.OnToggleFirstPerson += TryToggleControlsForFirstPerson;
        EventManager.instance.OnToggleTwoD += ToggleControlsForTwoD;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blurred"))
        {
            isInBlurredSpace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Blurred"))
        {
            isInBlurredSpace = false;
        }
    }

    private void TryToggleControlsForFirstPerson()
    {
        if (!isInBlurredSpace)
        {
            ToggleControlsForFirstPerson();
        }
        else
        {
            ProvideFeedback();
        }
    }

    private void ProvideFeedback()
    {
        StartCoroutine(cameraShake.Shake(0.15f, 0.4f));
        invalidSwitchSound.Play();
        StartCoroutine(ShowInvalidSwitchMessage());
    }

    private IEnumerator ShowInvalidSwitchMessage()
    {
        invalidSwitchMessage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        invalidSwitchMessage.SetActive(false);
    }

    private void ToggleControlsForTwoD()
    {
        twoDCharacterMovement.enabled = true;
        firstPersonCharacterMovement.enabled = false;
    }

    private void ToggleControlsForFirstPerson()
    {
        firstPersonCharacterMovement.enabled = true;
        twoDCharacterMovement.enabled = false;
    }
}