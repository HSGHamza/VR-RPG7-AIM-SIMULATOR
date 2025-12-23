using UnityEngine;
using TMPro;
using System.Collections;

public class RocketUIManager : MonoBehaviour
{
    public static RocketUIManager Instance;
    public TextMeshProUGUI statsText;
    public float fadeDelay = 4f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // stays alive even if scene reloads
    }

    public void ShowStats(string stats)
    {
        if (statsText == null)
        {
            Debug.LogWarning("RocketUIManager has no TextMeshProUGUI assigned!");
            return;
        }

        statsText.text = stats;

        // restart fade coroutine
        StopAllCoroutines();
        StartCoroutine(FadeOutAfterDelay());
    }

    IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(fadeDelay);
        statsText.text = "";
    }
}
