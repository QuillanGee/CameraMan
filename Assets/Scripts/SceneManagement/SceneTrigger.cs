using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public string sceneToLoad;   // Assign this in the Inspector
    public string sceneToUnload; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure your player has the "Player" tag
        {
            // Check if the scene is already loaded before loading
            if (!IsSceneLoaded(sceneToLoad))
            {
                EventManager.instance.LoadScene(sceneToLoad);
            }

            // Check if the scene is loaded before unloading
            if (IsSceneLoaded(sceneToUnload))
            {
                EventManager.instance.UnloadScene(sceneToUnload);
            }
        }
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded; // Returns true if the scene is already loaded
    }
}