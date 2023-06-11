using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarterEnemy : MonoBehaviour
{
    public TextMeshPro text;

    public Enemy enemy;

    public string textAfterDeath = "You will pay for this...";

    private void Start()
    {
        if(enemy == null)
            enemy = GetComponentInParent<Enemy>();
        if(text == null)
            text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if(enemy.dead)
            text.text = textAfterDeath;
    }
}
