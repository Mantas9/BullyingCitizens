using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Door gymDoor;

    public int currentWave = 0;

    public AudioSource peacefulSource;
    public AudioSource battleSource;

    public TextMeshProUGUI timerText;
    public float trainingTimeSeconds = 60;
    public float timeLeftToTrain = 0;

    public TextMeshProUGUI startTrainingText;
    public bool trainingSessionStarted = false;
    public bool trainingPart = false;
    public bool fightingPart = false;

    public UnityEvent enemyKilledEvent;
    public int enemiesKilled = 0;
    //public int enemiesSpawned = 0;

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
        ToggleGymDoor(false);
        peacefulSource.Play();
    }

    private void Update()
    {
        if (!trainingSessionStarted && trainingPart)
        {
            startTrainingText.gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.T))
            {
                startTrainingText.gameObject.SetActive(false);
                StartTraining();
            }
        }

        if(trainingPart && trainingSessionStarted)
        {
            timeLeftToTrain -= Time.deltaTime;
            timerText.text = timeLeftToTrain.ToString("0") + "s";

            if (timeLeftToTrain < 0)
                EndTraining();
        }

        if(!fightingPart && !peacefulSource.isPlaying)
        {
            peacefulSource.Play();
            battleSource.Pause();
        }
        else if(fightingPart && !battleSource.isPlaying)
        {
            battleSource.Play();
            peacefulSource.Pause();
        }
    }

    public void ToggleGymDoor(bool toggle = true)
    {
        gymDoor.doorEnabled = toggle;
    }

    public void EnterGym()
    {
        timerText.enabled = true;
        startTrainingText.enabled = true;
        trainingPart = true;
    }

    public void ExitGym()
    {
        trainingSessionStarted = false;
        trainingPart = false;
        fightingPart = true;
        timerText.enabled = false;
        startTrainingText.enabled = false;
        StartFightSection();

        ToggleGymDoor(false);
    }

    private void StartFightSection()
    {
        foreach (var item in spawners)
        {
            item.StartCoroutine(item.WaveRoutine());
        }
    }

    private void StartTraining()
    {
        timerText.enabled = true;
        trainingSessionStarted = true;
        timeLeftToTrain = trainingTimeSeconds;
    }

    private void EndTraining()
    {
        timerText.enabled = false;
        trainingSessionStarted = false;
        trainingPart = false;
    }

    private void EnemyKilled()
    {
        enemiesKilled++;
        RatHolder.instance.sacrificesMade++;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var item in enemies)
        {
            if (!item.GetComponent<Enemy>().dead)
                return;
        }

        fightingPart = false;
        ToggleGymDoor();
    }
}
