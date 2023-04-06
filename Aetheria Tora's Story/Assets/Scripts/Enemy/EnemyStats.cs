 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public HealthBar healthBar;
    public Canvas canvas;
    public EnemyManager enemyManager;
    public Vector3 originalPos;
    public bool hasBeenHit;
    float hitTimer = 0;
    public bool isDead = false;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        originalPos = transform.position;
    }

    private void Update()
    {
        if (hitTimer > 1)
        {
            hasBeenHit = false;
            hitTimer = 0;
        }

        if (hasBeenHit)
        {
            hitTimer += Time.deltaTime;
        }
        
    }

    private void LateUpdate()
    {
        if (isDead && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            currentHealth = maxHealth;
            enemyManager.currentTarget = null;
            enemyManager.enabled = true;
            canvas.enabled = true;
            transform.localPosition = originalPos;
            gameObject.SetActive(false);
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            //Destroy(gameObject);
        }
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private float SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10f;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetCurrentHealth(currentHealth);

        //animator.Play("TakeDamageFront");

        if (currentHealth <= 0)
        {
            enemyManager.enabled = false;
            canvas.enabled = false;
            currentHealth = 0;
            animator.Play("Death");
            isDead = true;


        }
        hasBeenHit = true;
    }
}
