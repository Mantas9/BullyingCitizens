using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public Movement movement;

    private void Start()
    {
        if(slider == null)
            slider = GetComponent<Slider>();

        slider.maxValue = movement.maxSprintTime;
    }

    private void Update()
    {
        slider.value = movement.timeSprintingRemaining;
    }
}
