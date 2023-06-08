using System;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public float maxHP = 100;
    public float hp;

    private void Start()
    {
        if (hp == 0)
            hp = maxHP;
    }

    private void Die()
    {
        if(TryGetComponent(out Enemy enemy) && !enemy.dead)
            enemy.Die();

        if(transform.gameObject.CompareTag("Player"))
            Destroy(transform.gameObject);   
    }

    public void Damage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
            Die();
    }
}
