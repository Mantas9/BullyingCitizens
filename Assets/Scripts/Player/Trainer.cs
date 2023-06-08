using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    // Punching
    [Header("Punching")]
    private Puncher puncher;
    public float punchesRequiredForLevelUp = 10;

    public float punchCooldownDecreaseMargin = 0.2f;
    public float punchKnockbackPowerIncreaseMargin = 0.05f;
    public int punchDamageIncreaseMargin = 5;

    // Sprinting
    [Header("Sprinting")]
    private Movement movement;
    public float runningTimeRequiredForLevelUp = 10;

    public float sprintSpeedIncreaseMargin = 0.1f;
    public float sprintEnduranceIncreaseMargin = 0.2f;
    public float sprintRecoveryTimeDecreaseMargin = 0.1f;

    private void Start()
    {
        if (puncher == null)
            puncher = GetComponentInChildren<Puncher>();
        if (movement == null)
            movement = GetComponent<Movement>();
    }
}
