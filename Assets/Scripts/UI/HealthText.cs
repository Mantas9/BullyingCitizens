using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Health health;

    public TextMeshProUGUI text;

    private void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = (int)health.hp + $"/{health.maxHP}";
    }
}
