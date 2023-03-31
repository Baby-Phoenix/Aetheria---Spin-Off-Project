 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public EnemyAttackAction lastAttack;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        HandleRotateTowardsTarget(enemyManager);

        if (enemyManager.isPerformingAction)
            return combatStanceState;

        if (currentAttack != null)
        {
            //If we are too close to the enemy to perform current attack, get a new attack
            if(enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            else if(enemyManager.distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
            {
                //If our enemy is withic our attack's viewable angle, we attack
                if(viewableAngle <= currentAttack.maximumAttackAngle && 
                    viewableAngle >= currentAttack.minimumAttackAngle)
                {
                    if(enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
                    {
                        enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
                        //enemyAnimatorManager.animator.SetFloat("horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatStanceState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return combatStanceState;
    }

    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {

                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;

                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                        lastAttack = currentAttack;
                    }
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
        {
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
}
