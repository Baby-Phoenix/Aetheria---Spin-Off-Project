using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimatorHandler enemyAnimatorHandler;
    NavMeshAgent navmeshAgent;
    AnimatorDataHandler enemyAnimatorDataHandler;
    public CharacterController enemyCharacterController;
    public CharacterStats currentTarget;
    public LayerMask detectionLayer;
    public float distanceFromTarget;
    public float stoppingDistance = 1;

    public float rotationSpeed = 15;

    private void Awake()
    {
        enemyAnimatorDataHandler = GetComponent<AnimatorDataHandler>();
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorHandler = GetComponent<EnemyAnimatorHandler>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyCharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        navmeshAgent.enabled = false;
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null )
            {
                Vector3 targetDirection = characterStats.transform.position- transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        //if (enemyManager.isPerformingAction)
        //    return;
        
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPerformingAction) 
        {
            //print("IN HERE");
            enemyAnimatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", 0, 0);
            navmeshAgent.enabled = false;
        }
        else
        {
            if(distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", 0, 0.9f);
            }
            else if(distanceFromTarget <= stoppingDistance)
            {
                enemyAnimatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", 0, 0);
            }
            
        }

        HandleRotateToTarget();

        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleRotateToTarget()
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
        }
        //Rotate with pathfinding 
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyCharacterController.velocity;

            navmeshAgent.enabled = true;
            navmeshAgent.SetDestination(currentTarget.transform.position);
            //enemyCharacterController.Move(targetVelocity);
            //enemyAnimatorDataHandler.UpdateAnimatorValues(targetVelocity.x, targetVelocity.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
    }
}
