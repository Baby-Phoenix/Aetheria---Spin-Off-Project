using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;
    public EnemyAnimatorManager enemyAnimatorManager;
    NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public PlayerStats currentTarget;
    public LayerMask detectionLayer;
    public LayerMask obstructionLayer;


    public float distanceFromTarget;
    public float stoppingDistance = 1;
    public float originalStoppingDistance;

    public float rotationSpeed = 15;
    public float speed = 1;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigidBody = GetComponent<Rigidbody>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        navmeshAgent.enabled = false;
        enemyRigidBody.isKinematic = false;
        originalStoppingDistance = stoppingDistance;

    }

    private void FixedUpdate()
    {
        //HandleGoBack(5);
    }

    private void Update()
    {
        navmeshAgent.stoppingDistance = stoppingDistance + 1;

        

        if (currentTarget != null && currentTarget.currentHealth <= 0 )
        {
            currentTarget = null;
        }
    }


    public void HandleGoBack(float distance)
    {
        float distanceToTarget = Vector3.Distance(transform.position, enemyStats.originalPos);

        if (distanceToTarget >= distance)
        {
            
            enemyManager.enemyLocomotionManager.currentTarget = null;
            navmeshAgent.SetDestination(enemyStats.originalPos);
            distanceFromTarget = -5;
            enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
            navmeshAgent.enabled = false;
            transform.localPosition = enemyStats.originalPos;
        }
    }

    public void HandleDetection()
    {
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
                    if (!Physics.Raycast(transform.position, targetDirection, distanceToTarget, obstructionLayer))
                        currentTarget = characterStats;


                }


            }
            

        }
    }

    public void HandleWarnNearbyEnemies()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            EnemyStats enemyStats = colliders[i].transform.GetComponent<EnemyStats>();
        
            if (enemyStats != null)
            {
                enemyStats.enemyManager.enemyLocomotionManager.currentTarget = currentTarget;
            }
        }
    }

    public void HandleMoveToTarget(Vector3 pos)
    {
        //if (enemyManager.isPerformingAction)
        //    return;
        
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(/*currentTarget.transform.position*/pos, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPerformingAction) 
        {
            enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
            navmeshAgent.enabled = false;
        }
        else
        {
            Vector3 direction = navmeshAgent.destination - transform.position;

            if (distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager.animator.SetFloat("vertical", 1, 0.1f, Time.deltaTime);

                
                
            }
            else if(distanceFromTarget <= stoppingDistance)
            {
                
                enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
            }
            
        }

        HandleRotateTowardsTarget(pos);

        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    public void ManualRotate()
    {
        if (!enemyManager.isPerformingAction || enemyManager.lastAttack.isAbleToRotate)
        {
            // Calculate the direction vector from the agent to the target
            Vector3 direction = currentTarget.transform.position - navmeshAgent.transform.position;
            direction.y = 0; // Ensure the direction is in the x-z plane only

            // Calculate the rotation angle in the y-axis using Atan2 function
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Set the agent's rotation to the calculated angle in the y-axis
            navmeshAgent.transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        //navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    public void HandleRotateTowardsTarget(Vector3 pos)
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = /*currentTarget.transform.position*/pos - transform.position;
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
            Vector3 targetVelocity = navmeshAgent.velocity;

            navmeshAgent.enabled = true;
            navmeshAgent.SetDestination(/*currentTarget.transform.position*/pos);
            enemyRigidBody.velocity = targetVelocity;

            //// Calculate the direction vector from the agent to the target
            //Vector3 direction = currentTarget.transform.position - navmeshAgent.transform.position;
            //direction.y = 0; // Ensure the direction is in the x-z plane only

            //// Calculate the rotation angle in the y-axis using Atan2 function
            //float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //// Set the agent's rotation to the calculated angle in the y-axis
            //navmeshAgent.transform.rotation = Quaternion.Euler(0, angle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);


        }
    }
}
