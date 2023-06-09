using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnitityHealth : MonoBehaviour
{
    public Health health;
    public TextMeshPro text;

    private void Start()
    {
        if (health == null)
            health = transform.parent.GetComponent<Health>();
        if(text == null)
            text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        text.text = $"{health.hp}/{health.maxHP}";
    }
}
