using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Wave
{
    public bool random = false;
    public float interval = 1f;
    public List<SpawnEntry> entries;
}

[Serializable]
public class SpawnEntry
{
    public int number;
    public GameObject enemy;
}

public class Spawner : MonoBehaviour
{
    public static List<Enemy> enemies = new();

    public List<Wave> waves;
    public int currentWave = 0;

    public bool spawning = false;

    public IEnumerator WaveRoutine()
    {
        // Start wave
        spawning = true;
        yield return StartCoroutine(SpawnRoutine());
        currentWave++;
        spawning = false;
    }

    private IEnumerator SpawnRoutine()
    {
        // Expand all entries into concrete prefab list
        var enemiesToSpawn = new List<GameObject>();
        foreach (var entry in waves[currentWave].entries)
        {
            for (int i = 0; i < entry.number; i++)
            {
                enemiesToSpawn.Add(entry.enemy);
            }
        }

        // Spawn until spawn list isnt empty
        while (enemiesToSpawn.Count > 0)
        {
            var prefab = enemiesToSpawn[0];

            if (waves[currentWave].random && enemiesToSpawn.Count != 1)
                prefab = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];

            enemiesToSpawn.Remove(prefab);
            Instantiate(prefab, transform.position, Quaternion.identity);
            
            yield return new WaitForSeconds(waves[currentWave].interval);
        }
    }
}
