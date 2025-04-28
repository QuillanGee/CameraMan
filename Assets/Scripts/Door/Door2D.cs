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

    public IEnumerator TriggerDoorAnimation()
    {
        current3DDoor.OpenLeftDoor();
        yield return new WaitForSeconds(1f);
        current3DDoor.CloseDoor();
    }

    public void TriggerCorrespondingDoorAnimation()
    {
        correspondingDoor.GetComponent<Animator>().SetBool("OpenLeft" , true);
    }
}
