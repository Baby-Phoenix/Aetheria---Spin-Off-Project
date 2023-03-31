using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;

    public LayerMask detectionLayer;
    public LayerMask obstructionLayer;
    public Transform lineOfSight;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        #region Handle Enemy Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerStats characterStats = colliders[i].transform.GetComponent<PlayerStats>();

            if (characterStats != null && characterStats.currentHealth > 0)
            {
                
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, characterStats.transform.position);
                    
                    //See if target is within eyesight
                    if (!Physics.Raycast(lineOfSight.position, targetDirection, 20, obstructionLayer))
                    {
                        enemyManager.currentTarget = characterStats;
                        return pursueTargetState;
                    }
                }
            }
        }

        #endregion

        #region Handle Swtich To Next State
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        #endregion
    }
}
