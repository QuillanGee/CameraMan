using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchEffect : MonoBehaviour
{
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
        GlitchRenderPass.SetGlitch(isPaused); // ✅ Activate on pause, disable on resume
    }
    
    
}