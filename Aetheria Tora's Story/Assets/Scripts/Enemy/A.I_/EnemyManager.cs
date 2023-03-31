using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public State currentState;
    public PlayerStats currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public float distanceFromTarget;
    //public float originalStoppingDistance;
    public float rotationSpeed = 15;
    public float maximumAttackRange = 1.5f;


    [Header("A.E Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;

    public float currentRecoveryTime = 0;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        navmeshAgent.enabled = false;
    }
    private void Start()
    {
        enemyRigidBody.isKinematic = false;
        //originalStoppingDistance = stoppingDistance;

    }

    private void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if(currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }

        //if(enemyLocomotionManager.currentTarget != null)
        //{
        //    enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

        //    enemyLocomotionManager.HandleWarnNearbyEnemies();

        //    if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance + 5)
        //    {
        //        //enemyAnimatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", 0, 0);
                
        //        enemyLocomotionManager.ManualRotate();
        //        Vector3 targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
        //        float distanceToTarget = Vector3.Distance(transform.position, enemyLocomotionManager.currentTarget.transform.position);
        //        if (!Physics.Raycast(transform.position, targetDirection, distanceToTarget, enemyLocomotionManager.obstructionLayer))
        //        {
        //            AttackTarget();
        //            enemyLocomotionManager.stoppingDistance = enemyLocomotionManager.originalStoppingDistance;
        //        }
        //        else
        //        {
        //            enemyLocomotionManager.stoppingDistance = 1;
        //        }
        //    }
        //}

        //if (enemyLocomotionManager.currentTarget == null)
        //{
        //    enemyLocomotionManager.HandleDetection();
        //}
        //else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance) 
        //{
        //    enemyLocomotionManager.HandleMoveToTarget(enemyLocomotionManager.currentTarget.transform.position);
            
        //}
        
        
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if(isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }    
}
