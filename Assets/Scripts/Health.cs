using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // General
    [Header("HP")]
    public float maxHP = 100;
    public float lowHP = 20;
    public float hp;

    // Regeneration
    [Header("Regeneration")]
    public bool regenerates = false;
    [Range(0, 2)] public float regenerationStrength = 0.5f;
    public bool hurt = false;
    public float hurtTimer = 0;
    public float timeUntilRegen = 5f;
    public bool regenerating = false;

    // Effects
    [Header("Effects")]
    public Image damageOverlay;
    public Image lowHpOverlay;

    private void Start()
    {
        if (hp == 0)
            hp = maxHP;
    }

    private void Update()
    {
        if (regenerates)
        {
            Regeneration();
        }

        if (CompareTag("Player"))
        {
            if (hp <= lowHP)
                lowHpOverlay.gameObject.SetActive(true);
            else
                lowHpOverlay.gameObject.SetActive(false);
        }
    }

    private void Regeneration()
    {
        if (hurt)
        {
            hurtTimer += Time.deltaTime;

            if (hurtTimer >= timeUntilRegen)
            {
                hurt = false;
                hurtTimer = 0;
                regenerating = true;
            }
        }

        if (regenerating)
        {
            hp = Mathf.Lerp(hp, maxHP, Time.deltaTime * regenerationStrength);

            if (hp >= maxHP - 0.5f)
            {
                hp = maxHP;
                regenerating = false;
            }
        }
    }

    private void Die()
    {
        if (TryGetComponent(out Enemy enemy) && !enemy.dead)
            enemy.StartCoroutine(enemy.Die());

        if (transform.gameObject.CompareTag("Player"))
            Destroy(transform.gameObject);
    }

    public void Damage(float damage)
    {
        hp -= damage;

        if (CompareTag("Player"))
        {
            StartCoroutine(HurtEffectRoutine());
        }

        if (regenerates)
        {
            hurt = true;
            regenerating = false;
            hurtTimer = 0;
        }

        if (hp <= 0)
            Die();
    }

    private IEnumerator HurtEffectRoutine()
    {
        damageOverlay.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        damageOverlay.gameObject.SetActive(false);
    }
}
