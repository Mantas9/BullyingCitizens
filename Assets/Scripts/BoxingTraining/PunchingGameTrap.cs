using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingGameTrap : MonoBehaviour
{
    public PunchingGames parent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CameraEffects.ShakeOnce(0.1f, 100);
            parent.EndAllGames();
        }
    }
}
