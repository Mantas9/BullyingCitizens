using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SprintTraining : MonoBehaviour
{
    private Trainer trainer;
    public TextMeshProUGUI upgradeText;

    public float intensity = 20;

    public float upgradeGoalTime = 3;
    private float timer = 0;

    private void Start()
    {
        trainer = Trainer.instance;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.trainingSessionStarted)
        {
            print("Player Running");

            timer += Time.deltaTime;
            collision.rigidbody.AddForce(-collision.transform.forward * intensity);


            if (timer >= upgradeGoalTime)
            {
                timer = 0;
                intensity += 5;
                trainer.UpgradeSprintingStats(trainer.sprintSpeedIncreaseMargin, trainer.sprintEnduranceIncreaseMargin, trainer.sprintRecoveryTimeDecreaseMargin);
                StartCoroutine(UpgradeTextRoutine());
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.trainingSessionStarted)
        {
            print("Player Running");

            timer += Time.deltaTime;
            collision.attachedRigidbody.AddForce(-collision.transform.forward * intensity);


            if (timer >= upgradeGoalTime)
            {
                timer = 0;
                intensity += 5;
                trainer.UpgradeSprintingStats(trainer.sprintSpeedIncreaseMargin, trainer.sprintEnduranceIncreaseMargin, trainer.sprintRecoveryTimeDecreaseMargin);
                StartCoroutine(UpgradeTextRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.instance.trainingSessionStarted)
        {
            timer = 0;
        }
    }

    private IEnumerator UpgradeTextRoutine()
    {
        upgradeText.gameObject.SetActive(true);
        upgradeText.text = "Speed Upgraded";

        yield return new WaitForSeconds(2f);

        upgradeText.gameObject.SetActive(false);
    }
}
