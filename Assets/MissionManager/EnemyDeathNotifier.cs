using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    private WaveController waveController;
    private bool reported = false;

    void Start()
    {
        waveController = FindObjectOfType<WaveController>();
    }

    public void Die()
    {
        ReportDeath();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        ReportDeath();
    }

    void ReportDeath()
    {
        if (reported)
            return;

        reported = true;

        if (waveController != null)
            waveController.RegisterEnemyDeath();
    }
}
