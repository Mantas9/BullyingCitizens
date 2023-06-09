using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Movement movement;

    private void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = movement.timeSprintingRemaining.ToString("0.00") + "s/" + movement.maxSprintTime.ToString() + "s";
    }
}
