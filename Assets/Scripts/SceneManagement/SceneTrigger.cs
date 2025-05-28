using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public string sceneToLoad;   // Assign this in the Inspector
    public string sceneToUnload; // Assign this in the Inspector
    [SerializeField] private GameObject spawnDoor;
    [SerializeField] private Transform spawnDoorTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure your player has the "Player" tag
        {
            // // Check if the scene is already loaded before loading
            // if (!IsSceneLoaded(sceneToLoad))
            // {
            //     SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            //     StartCoroutine(DelayedEventCallForSceneLoading());
            // }

            // Check if the scene is loaded before unloading
            if (IsSceneLoaded(sceneToUnload))
            {
                SceneManager.UnloadSceneAsync(sceneToUnload);
                StartCoroutine(DelayedEventCallForSceneLoading());
            }

            if (spawnDoor != null)
            {
                Instantiate(spawnDoor, spawnDoorTransform.position, spawnDoorTransform.rotation);
            }
        }
    }

    private IEnumerator DelayedEventCallForSceneLoading()
    {
        // yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneToLoad).isLoaded);
        yield return new WaitForSeconds(2f);
        EventManager.instance.LoadScene();
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded; // Returns true if the scene is already loaded
    }
}