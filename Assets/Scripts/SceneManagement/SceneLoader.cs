using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        EventManager.instance.OnLoadScene += LoadSceneFromString;
        EventManager.instance.OnUnloadScene += UnloadSceneFromString;
    }
    
    private void LoadSceneFromString(object sender, string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    private void UnloadSceneFromString(object sender, string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}