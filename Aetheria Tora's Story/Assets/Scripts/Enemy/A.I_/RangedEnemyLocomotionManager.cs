using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyLocomotionManager : MonoBehaviour
{
    RangedEnemyManager rangedEnemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;
    NavMeshAgent navmeshAgent;

    public CharacterStats currentTarget;
    public LayerMask detectionLayer;
    public float distanceFromTarget;
    public float stoppingDistance = 1;


    private void Awake()
    {
        rangedEnemyManager = GetComponent<RangedEnemyManager>();
        enemyAnimatorHandler = GetComponent<EnemyAnimatorHandler>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangedEnemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > rangedEnemyManager.minimumDetectionAngle && viewableAngle < rangedEnemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (rangedEnemyManager.isPerformingAction)
        {
            navmeshAgent.enabled = false;
        }
        else
        {
            Vector3 direction = navmeshAgent.destination - transform.position;

            if (distanceFromTarget > stoppingDistance)
            {
                //enemyCharacterController.Move(direction.normalized * speed * Time.deltaTime);
            }
            else if (distanceFromTarget <= stoppingDistance)
            {

                //enemyAnimatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", 0, 0);
            }

        }

        //HandleRotateToTarget();

        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }
}
