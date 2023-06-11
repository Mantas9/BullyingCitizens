using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatHolder : MonoBehaviour
{
    public static RatHolder instance;

    public Animator animator;
    public AudioSource source;
    public AudioClip activateRatClip;
    public AudioClip useRatClip;

    public int timesUsed = 0; // amount of times the ability was used

    public Vector2 activationGoal;
    private int realActivationGoal;
    public int sacrificesMade = 0;

    public bool hasRat = false;
    public bool ratFulfilled = false;
    public bool useRat = false;


    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void NewActivationGoal()
    {
        sacrificesMade = 0;
        realActivationGoal = (int)Random.Range(activationGoal.x, activationGoal.y);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (sacrificesMade >= realActivationGoal && hasRat)
            ActivateRat();

        if (ratFulfilled && Input.GetKeyDown(KeyCode.R))
            StartCoroutine(UseRat());

        animator.SetBool("HasRat", hasRat);
        animator.SetBool("RatActive", ratFulfilled);
        animator.SetBool("UseRat", useRat);
    }

    public void ActivateRat()
    {
        if(ratFulfilled || !hasRat)
            return;

        source.PlayOneShot(activateRatClip);

        ratFulfilled = true;
    }

    public void PickUpRat()
    {
        if (hasRat)
            return;

        hasRat = true;

        NewActivationGoal();
    }

    public IEnumerator UseRat()
    {
        if (useRat == true)
            yield break;

        timesUsed++;

        source.PlayOneShot(useRatClip, 1);

        useRat = true;
        ratFulfilled = false;
        hasRat = false;

        yield return new WaitForSeconds(1f);

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.dropAllowed = false;
            enemyScript.ratKill = true;
            enemyScript.StartCoroutine(enemyScript.Die());
        }

        useRat = false;
    }
}
