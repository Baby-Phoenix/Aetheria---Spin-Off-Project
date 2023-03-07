using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public Text CSCountText;
    public Text SFCountText;
    public Text CCCountText;

    private WaitForSeconds regenTicks = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        crystalSoulCount = 3;
        soulFragmentCount = 0;
        cleansingCrystalCount = 0;

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    public void Update()
    {
        //CSCountText.text = crystalSoulCount.ToString();
        //SFCountText.text = soulFragmentCount.ToString();
        //CCCountText.text = cleansingCrystalCount.ToString();
    }

    public float SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10f;
        return maxHealth;
    }

    public int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animatorHandler.PlayTargetAnimation("TakeDamageFront", true);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;

            animatorHandler.PlayTargetAnimation("DeathFront", true);

            //Handle Player Death
        }
    }

    public void UseCrystalSoul()
    {
        currentHealth += 0.35f * maxHealth;
        currentHealth = (currentHealth + (0.35f * maxHealth) > maxHealth) ? maxHealth : 0.35f * maxHealth;

        crystalSoulCount--;
    }

    public void UseSoulFragment()
    {
        // TODO
    }

    public void UseCleansingCrystal()
    {
        // TODO
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
