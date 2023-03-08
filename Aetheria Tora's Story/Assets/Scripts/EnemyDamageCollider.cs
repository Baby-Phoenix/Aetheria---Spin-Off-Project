using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollider : MonoBehaviour
{
    [SerializeField] Collider damageCollider;
    EnemyLocomotionManager enemyLocomotionManager;

    public float pushForce = 10;
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        //damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    private void PushBack()
    {
        Vector3 pushDirection = enemyLocomotionManager.currentTarget.animatorHandler.playerLocomotion.rigidbody.transform.position - transform.position;
        pushDirection.Normalize();

        enemyLocomotionManager.currentTarget.animatorHandler.playerLocomotion.rigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
    }


    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats.playerManager.isInvulnerable)
                return;

            if (playerStats != null && playerStats.currentHealth > 0)
            {
                playerStats.TakeDamage(currentWeaponDamage);

                AnimatorClipInfo[] clipInfo = enemyLocomotionManager.enemyAnimatorManager.animator.GetCurrentAnimatorClipInfo(0);
                string currentClipName = clipInfo[0].clip.name;

                if (currentClipName == "metarig_melee")
                {
                    PushBack();
                }
            }
        }

        //if (collision.tag == "Enemy")
        //{
        //    EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

        //    if (enemyStats != null)
        //    {
        //        if (!enemyStats.hasBeenHit)
        //        {
        //            enemyStats.TakeDamage(currentWeaponDamage);
        //        }
        //    }
        //}
    }
}
