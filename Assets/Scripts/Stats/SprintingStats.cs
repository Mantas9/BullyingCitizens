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
        text.text = "Sprinting:\r\n  Sprint Speed - " + player.sprintSpeed.ToString("0.00") + "\r\n  Endurance - " + player.maxSprintTime.ToString("0.00") + "s\r\n  Recovery Time - " + player.timeUntilRecoveryStarts.ToString("0.00") + "s";
    }
}
