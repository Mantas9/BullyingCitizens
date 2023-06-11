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
        text.text = "Punching:\r\n  Damage - " + player.damage.ToString() + "\r\n  Knockback - " + player.punchForce.ToString("0.00") + "\r\n  Punching interval - " + player.punchCooldown.ToString("0.00") + "s";
    }
}
