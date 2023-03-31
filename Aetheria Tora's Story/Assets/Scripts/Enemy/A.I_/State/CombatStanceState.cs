using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;

    public float teleportRadius = 5.0f;
    public float teleportChance = 0.1f;
    public float maxHeightDifference = 2.0f;
    public float teleportCooldown = 10.0f;
    public bool canTeleport = false;
    private float lastTeleportTime;

    private void Start()
    {
        lastTeleportTime = Time.time;
    }

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        //potentially circle player or walk around them

        HandleTeleportation(enemyManager);

        HandleRotateTowardsTarget(enemyManager);
        HandleWarnNearbyEnemies(enemyManager);
        //Check for attack range

        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        else if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    public void HandleWarnNearbyEnemies(EnemyManager enemyManager)
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 6, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            EnemyStats enemyStats = colliders[i].transform.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                enemyStats.enemyManager.currentTarget = enemyManager.currentTarget;
            }
        }
    }

    public void HandleTeleportation(EnemyManager enemyManager)
    {
        if (canTeleport && Time.time - lastTeleportTime >= teleportCooldown)
        {
            if (Random.value < teleportChance)
            {
                Vector3 randomDirection = Random.insideUnitSphere * teleportRadius;
                randomDirection += transform.position;

                // Check if the destination is within line of sight
                RaycastHit hit;
                if (Physics.Linecast(transform.position, randomDirection, out hit) && hit.collider.gameObject != gameObject)
                {
                    // Destination is not within line of sight, skip teleport
                    return;
                }

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(randomDirection, out navHit, teleportRadius, NavMesh.AllAreas))
                {
                    if (Mathf.Abs(navHit.position.y - transform.position.y) > maxHeightDifference)
                    {
                        // The new position has a height difference that's too great, try again.
                        return;
                    }

                    // Check if the destination has line of sight to the target
                    //if (Physics.Linecast(navHit.position, enemyManager.currentTarget.transform.position, out hit) && hit.collider.tag != "Player")
                    //{
                    //    print("!Player");
                    //    // Destination does not have line of sight to the target, skip teleport
                    //    return this;
                    //}

                    enemyManager.navmeshAgent.Warp(navHit.position);
                    enemyManager.transform.position = navHit.position;
                    lastTeleportTime = Time.time;
                }
            }
        }
    }


    public void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        //if (enemyManager.isPerformingAction)
        //{
        //    Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
        //    direction.y = 0;
        //    direction.Normalize();

        //    if (direction == Vector3.zero)
        //    {
        //        direction = transform.forward;
        //    }

        //    Quaternion targetRotation = Quaternion.LookRotation(direction);
        //    enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        //}
        ////Rotate with pathfinding 
        //else
        Vector3 targetVelocity = enemyManager./*enemyRigidBody.velocity;//*/navmeshAgent.velocity;
        enemyManager.navmeshAgent.enabled = true;
        enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyManager.enemyRigidBody.velocity = targetVelocity;
        Vector3 direction = (enemyManager.currentTarget.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, Time.deltaTime * 10);
        enemyManager.navmeshAgent.transform.rotation = Quaternion.Slerp(enemyManager.navmeshAgent.transform.rotation, targetRotation, Time.deltaTime * 10);
    }

    //public void HandleRotateTowardsTarget(EnemyManager enemyManager)
    //{
    //    //Rotate manually
    //    if (enemyManager.isPerformingAction)
    //    {
    //        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
    //        direction.y = 0;
    //        direction.Normalize();

    //        if (direction == Vector3.zero)
    //        {
    //            direction = transform.forward;
    //        }

    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
    //    }
    //    //Rotate with pathfinding 
    //    else
    //    {
    //        Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
    //        Vector3 targetVelocity = enemyManager./*enemyRigidBody.velocity;//*/navmeshAgent.velocity;

    //        enemyManager.navmeshAgent.enabled = true;
    //        enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
    //        enemyManager.enemyRigidBody.velocity = targetVelocity;
    //        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
    //    }
    //}
}
