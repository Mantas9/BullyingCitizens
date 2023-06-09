using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DitzeGames.Effects;
using System.Collections;
using TMPro;

public class Enemy : MonoBehaviour
{
    // Components
    [Header("Components")]
    private Animator animator;
    private Rigidbody rb;
    private NavMeshAgent agent;
    public Transform target;
    public Health health;
    //private GameManager manager;

    [Header("Effects")]
    public ParticleSystem deathParticles;
    public ParticleSystem hitParticles;
    public ParticleSystem ratKillParticles;
    public GameObject bloodPuddle;
    public LayerMask puddleMask;

    // Sounds
    [Header("Sounds")]
    private AudioSource source;
    public AudioClip hitSound;
    public AudioClip recoverSound;
    private bool recovered = true;
    public AudioClip deathSound;

    // Drops
    [Header("Drops")]
    public bool dropAllowed = true;
    [Range(0, 100)] public float dropChance = 50f;
    public GameObject dropPrefab;

    [Header("Life/Death")]
    public bool dead = false;
    public bool ratKill = false;
    public float damage = 20;

    [Header("Recovery")]
    public bool punched = false;
    public Vector2 recoveryTime = new Vector2(2, 10);
    private float currentRecoveryTime;
    private float recovering = 0f;

    [Header("Seeking")]
    public float maxViewRange = 30f;
    public float minViewRange = 2f;
    public float moveSpeed = 10f;
    public LayerMask mask;

    private void Start()
    {
        source = GetComponentInChildren<AudioSource>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        agent.speed = moveSpeed;
        target = GameObject.Find("Player").transform;

        NewRecoveryTime();
    }

    private void Update()
    {
        if (dead)
        {
            animator.SetBool("Dead", true);
            return;
        }
        else
        {
            animator.SetBool("Dead", false);
        }

        if (recovering >= currentRecoveryTime && punched)
        {
            var pos = transform.position;
            pos.y = 0;
            transform.position = pos;
            punched = false;
        }

        if (punched)
        {
            animator.SetBool("Punched", true);
            agent.enabled = false;
            rb.constraints = RigidbodyConstraints.None;
            recovering += Time.deltaTime;
        }
        else
        {
            animator.SetBool("Punched", false);
            if(!recovered)
            {
                recovered = true;
                source.PlayOneShot(recoverSound);
            }

            agent.enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            recovering = 0;
            FindTarget();
        }

    }

    public void NewRecoveryTime()
    {
        currentRecoveryTime = Random.Range(recoveryTime.x, recoveryTime.y);
    }

    public IEnumerator Die()
    {
        if (dead)
            yield break;

        dead = true;

        // Death Effect
        deathParticles.Play();
        StartCoroutine(SpawnBloodPuddle()); // Blood puddle
        source.Stop();

        //yield return null;
        //source.PlayOneShot(deathSound, 20);


        if (ratKill)
        {
            ratKillParticles.Play();

            yield return new WaitForSeconds(1f);

            ratKillParticles.Stop();
        }

        agent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Physics.IgnoreCollision(GetComponent<Collider>(), target.GetComponentInChildren<Collider>());
        animator.enabled = false;

        // Killed enemy event
        GameManager.instance.enemyKilledEvent.Invoke();

        // StarterEnemy
        if(GetComponentInChildren<TextMeshPro>().TryGetComponent<StarterEnemy>(out var starterEnemy))
        {
            GameManager.instance.ToggleGymDoor();
        }

        // Drops
        if (Random.Range(0, 100) <= dropChance && dropAllowed)
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator SpawnBloodPuddle()
    {
        yield return new WaitForSeconds(2f);
        //print("Spawned");

        var ray = new Ray(deathParticles.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit, 100, puddleMask))
            Instantiate(bloodPuddle, hit.point + new Vector3(0, 0.01f, 0), bloodPuddle.transform.rotation);
    }

    private void FindTarget()
    {
        var distance = Vector3.Distance(transform.position, target.position);

        if (distance > maxViewRange)
        {
            agent.enabled = false;
            animator.SetBool("Running", false);
            return;
        }

        var direction = (target.position - transform.position).normalized;
        var ray = new Ray(transform.position, direction);

        if (!Physics.Raycast(ray, out var hit, maxViewRange, mask))
        {
            agent.enabled = false;
            animator.SetBool("Running", false);
            return;
        }

        if (hit.collider.CompareTag("Player") && GetComponent<NavMeshAgent>().enabled)
        {
            agent.destination = target.position;
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dead) // Damage to player
        {
            //print(collision.name);
            collision.transform.parent.GetComponent<Health>().Damage(damage);
            CameraEffects.ShakeOnce(0.2f, 100);
        }

        if (collision.gameObject.CompareTag("Fist") && !punched) // Damage from player
        {
            //print("Hit");

            CameraEffects.ShakeOnce(0.2f, 100);

            hitParticles.Play();


            punched = true;
            recovered = false;
            var puncher = collision.gameObject.GetComponentInChildren<Puncher>();

            health.Damage(puncher.damage);
            var force = Random.Range(puncher.punchForce / 2, puncher.punchForce);

            agent.enabled = false;
            rb.constraints = RigidbodyConstraints.None;

            rb.AddForce(collision.transform.up * force + transform.right * force, ForceMode.Impulse);
            rb.MoveRotation(Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));

            source.PlayOneShot(hitSound);
        }
    }
}
