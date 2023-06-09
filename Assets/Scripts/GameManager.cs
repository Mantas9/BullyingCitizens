using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentWave = 0;

    public float trainingTimeSeconds = 60;

    public bool trainingPart = false;
    public bool fightingPart = false;

    public int enemiesKilled = 0;

    private void Awake()
    {
        instance = this;
    }
}
