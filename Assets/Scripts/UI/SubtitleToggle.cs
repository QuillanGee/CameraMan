using UnityEngine;

public class SubtitleToggle : MonoBehaviour
{
    public static SubtitleToggle Instance;

    // Your persistent check variable
    public bool check = true;

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

    private void Start()
    {
        EventManager.instance.OnTriggerLoadingScene += DestorySelf;
    }
    
    private void DestorySelf()
    {
        Destroy(gameObject);
    }
}