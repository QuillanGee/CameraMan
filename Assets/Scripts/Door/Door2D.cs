using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2D : MonoBehaviour
{
    private Transform correspondingDoor;
    private Door current3DDoor;

    public Transform getCorrespondingDoorTransform()
    {
        return correspondingDoor;
    }

    public void SetCorrepondingDoorTransform(Transform newCorrespondingDoorTransform)
    {
        correspondingDoor = newCorrespondingDoorTransform;
    }

    public void SetDoor(Door newDoor)
    {
        current3DDoor = newDoor;
    }

    public Door getDoor()
    {
        return current3DDoor;
    }
}
