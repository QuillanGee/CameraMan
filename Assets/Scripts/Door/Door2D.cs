using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2D : MonoBehaviour
{
    private GameObject correspondingDoor;
    private Door current3DDoor; 

    public GameObject getCorrespondingDoorTransform()
    {
        return correspondingDoor;
    }

    public void SetCorrepondingDoorTransform(GameObject newCorrespondingDoorTransform)
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

    public void TriggerDoorAnimation()
    {
        current3DDoor.OpenLeftDoor();
    }

    public void TriggerCorrespondingDoorAnimation()
    {
        correspondingDoor.GetComponent<Animator>().SetBool("OpenLeft" , true);
    }
}
