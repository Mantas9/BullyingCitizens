using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchPoint : MonoBehaviour
{
    public void GotHit()
    {
        var parent = transform.parent.GetComponentInChildren<PunchPointsSpawner>();

        parent.PunchPointHit(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("AvoidPunchPoint") && !CompareTag("AvoidPunchPoint"))
        {
            Destroy(other.gameObject);
        }
    }
}
