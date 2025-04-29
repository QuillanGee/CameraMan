using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinControls : MonoBehaviour
{
    public float spinSpeed = 100f; // degrees per second
    private Transform ObjectToSpin;
    private bool isSpinActive = false;

    private void Start()
    {
        EventManager.instance.OnEnableSpinControls += EnableSpinControls;
        EventManager.instance.OnDisableSpinControls += DisableSpinControls;
    }

    private void OnDestroy()
    {
        // Always good practice to unsubscribe when destroyed
        EventManager.instance.OnEnableSpinControls -= EnableSpinControls;
        EventManager.instance.OnDisableSpinControls -= DisableSpinControls;
    }

    private void Update()
    {
        if (isSpinActive)
        {
            float spinDirection = 0f;

            if (Input.GetKey(KeyCode.D))
            {
                spinDirection = -1f; // Spin right (clockwise)
            }
            else if (Input.GetKey(KeyCode.A))
            {
                spinDirection = 1f; // Spin left (counter-clockwise)
            }

            if (spinDirection != 0f)
            {
                ObjectToSpin.Rotate(Vector3.up, spinDirection * spinSpeed * Time.deltaTime);
            }
        }
    }

    private void EnableSpinControls(object sender, Transform spinableobject)
    {
        ObjectToSpin = spinableobject;
        isSpinActive = true;
    }

    private void DisableSpinControls()
    {
        isSpinActive= false;
    }
}