using UnityEngine;

public class PerspectiveLockArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PerspectiveLockManager.Instance.SetLock(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PerspectiveLockManager.Instance.SetLock(false);
        }
    }
}