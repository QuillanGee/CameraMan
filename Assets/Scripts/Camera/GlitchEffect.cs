using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    [SerializeField] Material glitchMaterial;
    private bool isGlitchAlive = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        EventManager.instance.OnPauseGamePlay += HandlePause;
    }

    private void HandlePause(object sender, bool isPaused)
    {
        isGlitchAlive = isPaused;
    }
    
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (isGlitchAlive && glitchMaterial != null)
        {
            Graphics.Blit(src, dest, glitchMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}