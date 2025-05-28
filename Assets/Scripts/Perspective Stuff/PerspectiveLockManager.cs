using UnityEngine;

public class PerspectiveLockManager : MonoBehaviour
{
    public static PerspectiveLockManager Instance { get; private set; }

    private bool isLocked = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsLocked() => isLocked;

    public void SetLock(bool value)
    {
        isLocked = value;
    }
}