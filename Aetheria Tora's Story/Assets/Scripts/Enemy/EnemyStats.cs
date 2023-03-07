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

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        originalPos = transform.localPosition;
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
        if (currentHealth <= 0/*animator.GetCurrentAnimatorStateInfo(1).IsName("DeathFront") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1 && !animator.IsInTransition(0)*/)
        {
            currentHealth = maxHealth;
            enemyManager.enemyLocomotionManager.currentTarget = null;
            enemyManager.enabled = true;
            canvas.enabled = true;
            transform.localPosition = originalPos;
            gameObject.SetActive(false);
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
            //animator.Play("DeathFront");
            
        }
        hasBeenHit = true;
    }
}
