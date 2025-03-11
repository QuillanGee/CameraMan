using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {

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