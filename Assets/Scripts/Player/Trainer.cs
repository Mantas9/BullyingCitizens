using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public static Trainer instance;

    // Punching
    [Header("Punching")]
    private Puncher puncher;
    public float punchCooldownDecreaseMargin = 0.2f;
    public float punchKnockbackPowerIncreaseMargin = 0.05f;
    public int punchDamageIncreaseMargin = 5;

    // Sprinting
    [Header("Sprinting")]
    private Movement movement;
    public float sprintSpeedIncreaseMargin = 0.1f;
    public float sprintEnduranceIncreaseMargin = 0.2f;
    public float sprintRecoveryTimeDecreaseMargin = 0.1f;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        if (puncher == null)
            puncher = GetComponentInChildren<Puncher>();
        if (movement == null)
            movement = GetComponent<Movement>();
    }

    public void UpgradePunchingStats(float punchCooldown = 0, float punchKnockbackForce = 0, int punchDamage = 0)
    {
        puncher.punchCooldown -= punchCooldown;
        puncher.punchForce += punchKnockbackForce;
        puncher.damage += punchDamage;
    }

    public void UpgradeSprintingStats(float sprintingSpeed = 0, float sprintingEndurance = 0, float sprintingRecoveryTime = 0)
    {
        movement.sprintSpeed += sprintingSpeed;
        movement.maxSprintTime += sprintingEndurance;
        movement.timeUntilRecoveryStarts -= sprintingRecoveryTime;
    }
}
