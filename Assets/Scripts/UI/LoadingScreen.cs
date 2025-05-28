using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen; // Your panel
    public Slider progressBar;       // Optional
    public Text progressText;        // Optional
    [SerializeField] private string scene;

    void Start()
    {
        EventManager.instance.OnTriggerLoadingScene += LoadScene;
    }
    
    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            // Unity loads to 0.9, then waits for activation
            if (operation.progress >= 0.9f)
            {
                // Optionally wait for user input or time
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        // EventManager.instance.PostTriggerLoadingScene();
    }
}