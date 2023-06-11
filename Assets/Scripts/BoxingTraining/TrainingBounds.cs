using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBounds : MonoBehaviour
{
    public PunchingGames parent;

    private void Start()
    {
        if(parent == null)
            parent = transform.parent.GetComponent<PunchingGames>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            parent.EndAllGames();
        }
    }
}
