 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public HealthBar healthBar;
    public Canvas canvas;
    EnemyManager enemyManager;

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
    }
    private void LateUpdate()
    {
        if (currentHealth <= 0/*animator.GetCurrentAnimatorStateInfo(1).IsName("DeathFront") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1 && !animator.IsInTransition(0)*/)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
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
            //animator.Play("DeathFront");
            
        }
    }
}
