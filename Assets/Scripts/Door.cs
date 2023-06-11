using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject destination;
    public bool doorEnabled = true;
    public bool doorToGym = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") || !doorEnabled)
            return;

        if (doorToGym)
            GameManager.instance.EnterGym();
        else
            GameManager.instance.ExitGym();

        var enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Clear enemies from Arena
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        collision.transform.position = destination.transform.position;
    }
}
