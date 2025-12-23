using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Prevent automatic scene activation
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is fully loaded (progress 0.9)
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log($"Loading progress: {asyncLoad.progress * 100}%");
            yield return null;
        }

        // Scene is fully loaded, activate it
        asyncLoad.allowSceneActivation = true;

        // Optional: Wait until scene activation is complete
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene loaded and activated!");
    }
}