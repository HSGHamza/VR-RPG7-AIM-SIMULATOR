using UnityEngine;
using TMPro;
using System.Collections;

public class WaveDisplay : MonoBehaviour
{
    public TextMeshProUGUI waveText; // Assign in inspector
    public float displayTime = 2f;   // How long the text is visible

    private Coroutine currentCoroutine;

    public void ShowWave(int waveNumber)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        waveText.text = "Wave " + waveNumber;
        waveText.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        waveText.gameObject.SetActive(false);
    }
}
