using UnityEngine;

public class SubtitleToggle : MonoBehaviour
{
    public static SubtitleToggle Instance;

    // Your persistent check variable
    public bool check = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}