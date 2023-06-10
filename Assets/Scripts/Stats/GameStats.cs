using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public TextMeshPro text;

    private int currentWave;
    private int enemiesKilled;
    private int timesAbilityUsed;

    private void Start()
    {
        if(text == null)
            text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        currentWave = GameManager.instance.currentWave + 1;
        enemiesKilled = GameManager.instance.enemiesKilled;
        timesAbilityUsed = RatHolder.instance.timesUsed;

        text.text = $"Game Statistics:\r\n\r\n  Current Wave - {currentWave}\r\n\r\n  Enemies Killed - {enemiesKilled}\r\n\r\n  Ability Used - {timesAbilityUsed}";
    }
}
