using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PunchingGames : MonoBehaviour
{
    public Transform player;
    public Transform playerStandingPoint;

    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI upgradedNotification;

    public GameObject mainFist;
    public GameObject trainingFist;

    public bool minigameInProgress = false;
    private PunchPointsSpawner pointSpawner;

    // Speed minigame
    [Header("Speed Minigame")]
    public float completionTime;
    public int pointsToSpawn = 10;

    // Power minigame
    [Header("Power Minigame")]
    public float intervalTime = 2;
    public int sectionsRequiredForLevelUp = 3;

    private void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;
        if (mainFist == null)
            mainFist = GameObject.Find("Fist");
        if (trainingFist == null)
            trainingFist = GameObject.Find("TrainingFist");
        if (pointSpawner == null)
            pointSpawner = GetComponentInChildren<PunchPointsSpawner>();
    }

    private void Update()
    {
        if (!GameManager.instance.trainingPart)
            EndAllGames();
    }

    public void EndAllGames()
    {
        if (!minigameInProgress)
            return;

        EndPowerMiniGame();
        EndSpeedMiniGame();
    }

    private void ToggleTrainingFist(bool toggle)
    {
        trainingFist.SetActive(toggle);
        mainFist.SetActive(!toggle);
    }

    public void StartPowerMinigame()
    {
        if (minigameInProgress)
            return;

        minigameInProgress = true;
        ToggleTrainingFist(true);
        StartCoroutine(StartPowerMinigameRoutine());
    }

    private IEnumerator StartPowerMinigameRoutine()
    {
        GetPlayerReady();

        yield return CountdownRoutine(3);

        pointSpawner.powerGameInterval = intervalTime;
        pointSpawner.gameInProgress = true;
        pointSpawner.StartCoroutine(pointSpawner.PowerGameSpawn(sectionsRequiredForLevelUp));
    }

    public void UpgradePower()
    {
        var trainer = Trainer.instance;
        trainer.UpgradePunchingStats(0, trainer.punchKnockbackPowerIncreaseMargin, trainer.punchDamageIncreaseMargin);
        StartCoroutine(UpgradeTextRoutine("Power Upgraded"));
        print("Power Upgraded");
    }

    private IEnumerator UpgradeTextRoutine(string text)
    {
        upgradedNotification.gameObject.SetActive(true);
        upgradedNotification.text = text;

        yield return new WaitForSeconds(2f);

        upgradedNotification.gameObject.SetActive(false);
    }

    public void EndPowerMiniGame()
    {
        pointSpawner.gameInProgress = false;
        pointSpawner.ClearPoints();
        minigameInProgress = false;
        ToggleTrainingFist(false);
        print("Ended Minigame");
    }

    public void StartSpeedMiniGame()
    {
        if (minigameInProgress)
            return;

        minigameInProgress = true;
        ToggleTrainingFist(true);
        StartCoroutine(StartSpeedMinigameRoutine());
    }

    private IEnumerator StartSpeedMinigameRoutine()
    {
        GetPlayerReady();

        yield return CountdownRoutine(3);

        pointSpawner.speedGame = true;
        pointSpawner.SpeedGameSpawn(pointsToSpawn, true);

        if(completionTime <= 0)
            completionTime = pointsToSpawn / 2;

        yield return CountdownRoutine(completionTime, true);

        if (minigameInProgress)
        {
            EndSpeedMiniGame();
        }
    }

    public void EndSpeedMiniGame(bool successful = false)
    {
        if (successful)
        {
            var trainer = Trainer.instance;
            trainer.UpgradePunchingStats(trainer.punchCooldownDecreaseMargin);
            StartCoroutine(UpgradeTextRoutine("Punching Speed Upgraded"));
            print("Speed Upgraded");
        }

        pointSpawner.speedGame = false;
        pointSpawner.ClearPoints();
        minigameInProgress = false;
        ToggleTrainingFist(false);
        print("Ended Minigame");
    }

    public IEnumerator CountdownRoutine(float time, bool numbersAfterDecimal = false)
    {
        countDownText.gameObject.SetActive(true);
        for (float i = time; i > 0; i -= Time.deltaTime)
        {
            if (!minigameInProgress)
                break;

            string output = i.ToString("0");
            if(numbersAfterDecimal)
                output = i.ToString("0.00");

            countDownText.text = output;
            yield return null;
        }
        countDownText.gameObject.SetActive(false);
    }

    private void GetPlayerReady()
    {
        var movement = player.GetComponent<Movement>();
        movement.enabled = false;
        player.transform.position = playerStandingPoint.transform.position;
        player.transform.LookAt(transform.position);
        movement.enabled = true;
    }
}
