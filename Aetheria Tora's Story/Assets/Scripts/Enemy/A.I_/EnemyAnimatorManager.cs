using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;

    public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed;

    [SerializeField] EnemyDamageCollider damageCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyManager =  GetComponentInParent<EnemyManager>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;
        if(animator.GetBool("isInteracting"))
            enemyManager.navmeshAgent.velocity = velocity;
    }

    public void OpenDamageCollider()
    {
        damageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        damageCollider.DisableDamageCollider();
    }

    public void Shoot()
    {
        GameObject clone = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody rb = clone.GetComponentInChildren<Rigidbody>();

        Vector3 direction = enemyManager.currentTarget.transform.position - spawnPoint.position;
        direction.y += 1;
        direction.Normalize();

        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        if(clone != null)
        {
            Destroy(clone, 3);
        }
       
    }
}
