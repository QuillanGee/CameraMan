using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticZAxisFor2DLevel : MonoBehaviour
{
    public static StaticZAxisFor2DLevel Instance { get; private set; }
    public static Transform currentZAxis { get; private set; }
    [SerializeField] private Transform initialZAxis;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        EventManager.instance.OnLoadScene += AttachProjectedWallTransform;
        currentZAxis = initialZAxis;
    }

    private void AttachProjectedWallTransform()
    {
        GameObject zAxisForLevel = GameObject.FindWithTag("zAxisFor2DLevel");
        if (zAxisForLevel != null)
        {
            currentZAxis = zAxisForLevel.transform;
            Debug.LogWarning("ProjectedWallTransform found in scene!");
        }
        else
        {
            Debug.LogWarning("ProjectedWallTransform not found in scene!");
        }
    }

    public void SetZAxis(Transform zAxis)
    {
        currentZAxis = zAxis;
    }
}
