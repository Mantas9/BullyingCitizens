using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatHolder : MonoBehaviour
{
    public static RatHolder instance;

    public Animator animator;

    public Vector2 activationGoal;

    public bool hasRat = false;
    public bool ratFulfilled = false;
    public bool useRat = false;


    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        animator.SetBool("HasRat", hasRat);
        animator.SetBool("RatActive", ratFulfilled);
    }

    public void ActivateRat()
    {
        if(ratFulfilled || !hasRat)
            return;

        ratFulfilled = true;
    }

    public void PickUpRat()
    {
        if (hasRat)
            return;

        hasRat = true;
        
    }

    public void UseRat()
    {
        useRat = true;

        animator.SetBool("UseRat", useRat);
    }

    private void LateUpdate()
    {
        useRat = false;
    }
}
