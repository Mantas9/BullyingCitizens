using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interractor : MonoBehaviour
{
    // Rat
    public GameObject pickUpRatUI;
    private bool showRatUI;

    private void Update()
    {
        RatUI();
    }

    private void RatUI()
    {
        if (pickUpRatUI == null)
            return;

        pickUpRatUI.SetActive(showRatUI);
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CollectableRat")) // If is in the collectable rat field
        {
            showRatUI = false;
        }
    }
}
