using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAnimatorManager : AnimatorManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    //AnimatorDataHandler enemyAnimatorDataHandler;

    public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed;

    [SerializeField] EnemyDamageCollider damageCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLocomotionManager =  GetComponentInParent<EnemyLocomotionManager>();
        //enemyAnimatorDataHandler = GetComponent<AnimatorDataHandler>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyLocomotionManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyLocomotionManager.enemyRigidBody.velocity = velocity;
        //enemyAnimatorDataHandler.UpdateAnimatorValues(velocity.x, velocity.z);
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

        Vector3 direction = enemyLocomotionManager.currentTarget.transform.position - spawnPoint.position;
        direction.y += 1;
        direction.Normalize();
        
        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        Destroy(clone, 3);
    }
}
