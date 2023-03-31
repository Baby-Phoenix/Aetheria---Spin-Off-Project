using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public LayerMask detectionLayer;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyAnimatorManager.animator.SetFloat("vertical", 1, 0.1f, Time.deltaTime);
        }

        //if (enemyManager.isPerformingAction)
        //{
        //    enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        //    enemyManager.navmeshAgent.enabled = false;
        //}
        //else
        //{
        //    //Vector3 direction = navmeshAgent.destination - transform.position;

        //    if (distanceFromTarget > enemyManager.stoppingDistance)
        //    {
        //        enemyAnimatorManager.animator.SetFloat("vertical", 1, 0.1f, Time.deltaTime);
        //    }
        //    else if (distanceFromTarget <= enemyManager.stoppingDistance)
        //    {
        //        enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        //    }

        //}
        HandleWarnNearbyEnemies(enemyManager);
        HandleRotateTowardsTarget(enemyManager);

        enemyManager.navmeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navmeshAgent.transform.localRotation = Quaternion.identity;

        if(enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatStanceState;
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
}
