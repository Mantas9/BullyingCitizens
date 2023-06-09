using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Health health;

    private void Start()
    {
        slider = GetComponent<Slider>();

        slider.maxValue = health.maxHP;
    }

    private void Update()
    {
        slider.value = health.hp;
    }
}
