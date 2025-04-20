using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2D : MonoBehaviour
{
    private Transform correspondingDoor;

    public Transform getCorrespondingDoorTransform()
    {
        return correspondingDoor;
    }

    public void SetCorrepondingDoorTransform(Transform newCorrespondingDoorTransform)
    {
        correspondingDoor = newCorrespondingDoorTransform;
    }
}
