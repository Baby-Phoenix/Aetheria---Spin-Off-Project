using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;

    private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
    private Coroutine regen;

    AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    public int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animatorHandler.PlayTargetAnimation("TakeDamageFront", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            animatorHandler.PlayTargetAnimation("DeathFront", true);

            //Handle Player Death
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;

        staminaBar.SetCurrentStamina(currentStamina);
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
            if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(StaminaRegen());
    }

    private IEnumerator StaminaRegen()
    {
        yield return new WaitForSeconds(2);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.SetCurrentStamina(currentStamina);
            yield return regenTicks;
        }

        regen = null;
    }

}
