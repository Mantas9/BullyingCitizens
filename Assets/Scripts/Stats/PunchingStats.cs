using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PunchingStats : MonoBehaviour
{
    public Puncher player;

    public TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        text.text = $"Punching:\r\n  Damage - {player.damage}\r\n  Knockback - {player.punchForce}\r\n  Punching interval - {player.punchCooldown}s";
    }
}
