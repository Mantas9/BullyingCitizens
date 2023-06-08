using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DitzeGames.Effects;

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
    public ParticleSystem deathParticles;
    public ParticleSystem hitParticles;

    // Drops
    [Header("Drops")]
    [Range(0, 100)] public float dropChance = 50f;
    public GameObject dropPrefab;

    [Header("Life/Death")]
    public bool dead = false;
    public float damage = 20;
    public AudioClip deathSound;

    [Header("Recovery")]
    public bool punched = false;
    public float minRecoveryTime = 2f; // Max time to recover from slap
    public float maxRecoveryTime = 10f; // Max time to recover from slap
    private float recoveryTime;
    private float recovering = 0f;

    [Header("Seeking")]
    public float maxViewRange = 30f;
    public float minViewRange = 2f;
    public float moveSpeed = 10f;
    public LayerMask mask;

    private void Start()
    {
        //manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

        if (recovering >= recoveryTime && punched)
            punched = false;

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
            agent.enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            recovering = 0;
            FindTarget();
        }

    }

    public void NewRecoveryTime()
    {
        recoveryTime = Random.Range(minRecoveryTime, maxRecoveryTime);
    }

    public void Die()
    {
        dead = true;
        agent.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        animator.enabled = false;

        deathParticles.Play();

        //if (deathSound != null)
            //SoundManager.PlaySound(deathSound, Random.Range(-8, 8));

        if (Random.Range(0, 100) <= dropChance)
            Instantiate(dropPrefab, transform.position, Quaternion.identity);

        //if (!gameEnd)
            //manager.onKill.Invoke();

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dead)
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
            CameraEffects.ShakeOnce(0.2f, 100);
        }

        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Fist"))
        {
            print("Hit");

            CameraEffects.ShakeOnce(0.2f, 100);

            hitParticles.Play();
            

            punched = true;
            var puncher = collision.gameObject.GetComponentInChildren<Puncher>();

            health.Damage(puncher.damage);
            var force = Random.Range(puncher.punchForce / 2, puncher.punchForce);

            agent.enabled = false;
            rb.constraints = RigidbodyConstraints.None;

            rb.AddForce(collision.transform.up * force + transform.right * force, ForceMode.Impulse);
            rb.MoveRotation(Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));
        }
    }
}
