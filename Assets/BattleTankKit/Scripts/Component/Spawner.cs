using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public bool waveControlled = true;
    public GameObject[] Object;
    public Vector3 Offset = new Vector3(0, 0.1f, 0);

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void SpawnWave(int count)
    {
        ClearSpawnedList();

        for (int i = 0; i < count; i++)
        {
            SpawnSingle();
        }
    }

    void SpawnSingle()
    {
        if (Object.Length == 0)
            return;

        GameObject spawnPick = Object[Random.Range(0, Object.Length)];

        Vector3 spawnPoint = DetectGround(
            transform.position +
            new Vector3(
                Random.Range(-transform.localScale.x / 2f, transform.localScale.x / 2f),
                0,
                Random.Range(-transform.localScale.z / 2f, transform.localScale.z / 2f)
            )
        );

        GameObject enemy = Instantiate(spawnPick, spawnPoint, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }

    void ClearSpawnedList()
    {
        spawnedEnemies.RemoveAll(item => item == null);
    }

    Vector3 DetectGround(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, float.MaxValue))
        {
            return hit.point + Offset;
        }
        return position;
    }
}
