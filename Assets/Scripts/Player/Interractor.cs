using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interractor : MonoBehaviour
{
    // Boxing Bag
    [Header("Boxing Bag")]
    public GameObject boxingBagUI;
    private bool showBoxingUI;

    // Rat
    [Header("Rat")]
    public GameObject pickUpRatUI;
    private bool showRatUI;

    private void Update()
    {
        PopUpUI(pickUpRatUI, showRatUI); // Rat UI
        PopUpUI(boxingBagUI, showBoxingUI); // Boxing Bag UI
    }

    private void PopUpUI(GameObject UI, bool active)
    {
        if (UI == null)
            return;

        UI.SetActive(active);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("CollectableRat")) // If is in the collectable rat field
        {
            showRatUI = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                RatHolder.instance.PickUpRat(); // Pick up the rat animation

                showRatUI = false;

                Destroy(other.gameObject);
            }
        }

        if(other.gameObject.CompareTag("BoxingBag") && GameManager.instance.trainingSessionStarted)
        {
            if (other.transform.GetComponent<PunchingGames>().minigameInProgress)
                return;

            showBoxingUI = true;

            if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) // Power/damage minigame
            {
                showBoxingUI = false;
                var powerGame = other.GetComponent<PunchingGames>();
                powerGame.StartPowerMinigame();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) // Speed minigame
            {
                showBoxingUI = false;

                var speedGame = other.GetComponent<PunchingGames>();
                speedGame.StartSpeedMiniGame();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CollectableRat"))
        {
            showRatUI = false;
        }
        if (other.gameObject.CompareTag("BoxingBag"))
        {
            showBoxingUI = false;
        }
    }
}
