using UnityEngine;
using UnityEngine.Events;
using BigRookGames.Weapons;


public class WaveController : MonoBehaviour
{
    public WaveDisplay waveDisplay; // Assign your WaveDisplay script in inspector

    public int currentWave = 0;
    public Spawner enemySpawner;

    public UnityEvent<int> onWaveStart;
    public UnityEvent<int> onWaveComplete;

    private int enemiesAlive = 0;
    private bool waveRunning = false;

    void Start()
    {
        StartNextWave();
    }


    public void StartNextWave()
    {
        if (waveRunning)
            return;

        currentWave++;
        waveRunning = true;

        int enemyCount = currentWave * 3;
        enemiesAlive = enemyCount;

        GunfireController gun = FindObjectOfType<GunfireController>();
        if (gun != null)
            gun.SetAmmo(999);

        onWaveStart?.Invoke(currentWave);

        // Show the wave number in front of the player
        if (waveDisplay != null)
            waveDisplay.ShowWave(currentWave);

        enemySpawner.SpawnWave(enemyCount);
    }


    public void RegisterEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            waveRunning = false;
            onWaveComplete?.Invoke(currentWave);
            StartNextWave(); // start immediately
        }
    }
}
