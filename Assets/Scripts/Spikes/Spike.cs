using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        EventManager.instance.ResetAlan();
    }
}
