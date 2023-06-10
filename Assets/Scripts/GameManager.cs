using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentWave = 0;

    public float trainingTimeSeconds = 60;
    public float timeLeftToTrain = 0;

    public bool trainingPart = false;
    public bool fightingPart = false;

    public UnityEvent enemyKilledEvent;
    public int enemiesKilled = 0;

    public List<Spawner> spawners;

    private void Awake()
    {
        instance = this;

        if (enemyKilledEvent == null)
            enemyKilledEvent = new UnityEvent();
    }

    private void Start()
    {
        enemyKilledEvent.AddListener(EnemyKilled);

        
    }

    private void StartSpawning()
    {
        foreach (var item in spawners)
        {
            item.StartCoroutine(item.WaveRoutine());
        }
    }

    private void EnemyKilled()
    {
        enemiesKilled++;
        RatHolder.instance.sacrificesMade++;
    }
}
