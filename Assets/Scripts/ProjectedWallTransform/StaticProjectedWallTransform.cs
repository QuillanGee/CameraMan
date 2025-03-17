using UnityEngine;

public class StaticProjectedWallTransform : MonoBehaviour
{
    public static StaticProjectedWallTransform Instance { get; private set; }
    public static Transform ProjectedWallTransform { get; private set; }

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
        AttachProjectedWallTransform(); // Initial assignment in the first scene
    }

    private void AttachProjectedWallTransform()
    {
        GameObject wall = GameObject.FindWithTag("ProjectedWallTransform");
        if (wall != null)
        {
            ProjectedWallTransform = wall.transform;
            Debug.LogWarning("ProjectedWallTransform found in scene!");
        }
        else
        {
            Debug.LogWarning("ProjectedWallTransform not found in scene!");
        }
    }
}