using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{
    // Animation
    public Animator animator;
    public bool punching = false;

    public bool canPunch = true;
    public float punchCooldown = 3f;
    private float cooldown;

    public float punchForce = 30; // Knockback
    public int damage = 40;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Punching();

        Physics.IgnoreLayerCollision(3, 2);

        animator.SetBool("Punching", punching);
    }

    private void Punching()
    {
        punching = false;

        if (!canPunch)
        {
            cooldown -= Time.deltaTime;

            if (cooldown > 0)
                return;

            canPunch = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            punching = true;
            canPunch = false;
            cooldown = punchCooldown;
        }
    }
}
