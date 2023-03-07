using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] Collider damageCollider;
    EnemyLocomotionManager enemyLocomotionManager;

    public float pushForce = 10;
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        FindObjectOfType<AudioManager>().Play("Laser");
    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null && playerStats.currentHealth > 0)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }

    }
}
