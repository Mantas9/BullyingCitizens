using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SprintingStats : MonoBehaviour
{
    public Movement player;

    public TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        text.text = $"Sprinting:\r\n  Sprint Speed - {player.sprintSpeed}\r\n  Endurance - {player.maxSprintTime}s\r\n  Recovery Time - {player.sprintRecoveryTime}s";
    }
}
