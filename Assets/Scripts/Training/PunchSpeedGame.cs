using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSpeedGame : MonoBehaviour
{
    public Transform player;
    public Transform playerStandingPoint;

    public GameObject mainFist;
    public GameObject trainingFist;

    public bool minigameInProgress = false;
    private PunchPointsSpawner pointSpawner;

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
        //Physics.IgnoreLayerCollision(2, 3);

        if (player.transform.position != playerStandingPoint.position && minigameInProgress)
            EndMiniGame();
    }

    public void EndMiniGame(bool successful = false)
    {
        if (successful)
        {
            var trainer = Trainer.instance;
            trainer.UpgradePunchingStats(trainer.punchCooldownDecreaseMargin);
            print("Speed Upgraded");
        }

        pointSpawner.ClearPoints();
        minigameInProgress = false;
        ToggleTrainingFist(false);
        print("Ended Minigame");
    }

    private void ToggleTrainingFist(bool toggle)
    {
        trainingFist.SetActive(toggle);
        mainFist.SetActive(!toggle);
    }

    public void StartMiniGame()
    {
        if (minigameInProgress)
            return;

        minigameInProgress = true;
        ToggleTrainingFist(true);
        StartCoroutine(StartMinigameRoutine());
    }

    private IEnumerator StartMinigameRoutine()
    {
        GetPlayerReady();
        print("3, 2, 1... GO");
        yield return new WaitForSeconds(3f);
        pointSpawner.SpeedGameSpawn(10, true);
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
