using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public string sceneToLoad;  // Assign this in the Inspector
    public string sceneToUnload;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure your player has the "Player" tag
        {
            EventManager.instance.LoadScene(sceneToLoad);
            EventManager.instance.UnloadScene(sceneToUnload);
        }
    }
}