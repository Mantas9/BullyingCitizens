using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interractor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("CollectableRat")) // If is in the collectable rat field
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                RatHolder.instance.PickUpRat(); // Pick up the rat animation
                Destroy(other.gameObject);
            }
        }
    }
}
