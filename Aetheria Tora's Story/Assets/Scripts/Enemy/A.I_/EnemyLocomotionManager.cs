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

    public float speed = 1;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        //enemyStats = GetComponent<EnemyStats>();
    }



    private void FixedUpdate()
    {
        //HandleGoBack(5);
    }

    private void Update()
    {
        //navmeshAgent.stoppingDistance = stoppingDistance + 1;

        

        //if (currentTarget != null && currentTarget.currentHealth <= 0 )
        //{
        //    currentTarget = null;
        //}
    }


    //public void HandleGoBack(float distance)
    //{
    //    float distanceToTarget = Vector3.Distance(transform.position, enemyStats.originalPos);

    //    if (distanceToTarget >= distance)
    //    {
            
    //        enemyManager.enemyLocomotionManager.currentTarget = null;
    //        navmeshAgent.SetDestination(enemyStats.originalPos);
    //        distanceFromTarget = -5;
    //        enemyAnimatorManager.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
    //        navmeshAgent.enabled = false;
    //        transform.localPosition = enemyStats.originalPos;
    //    }
    //}


    //public void HandleWarnNearbyEnemies()
    //{
        
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, 5, detectionLayer);

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        EnemyStats enemyStats = colliders[i].transform.GetComponent<EnemyStats>();
        
    //        if (enemyStats != null)
    //        {
    //            enemyStats.enemyManager.enemyLocomotionManager.currentTarget = currentTarget;
    //        }
    //    }
    //}

    //public void ManualRotate()
    //{
    //    if (!enemyManager.isPerformingAction || enemyManager.lastAttack.isAbleToRotate)
    //    {
    //        // Calculate the direction vector from the agent to the target
    //        Vector3 direction = currentTarget.transform.position - navmeshAgent.transform.position;
    //        direction.y = 0; // Ensure the direction is in the x-z plane only

    //        // Calculate the rotation angle in the y-axis using Atan2 function
    //        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

    //        // Set the agent's rotation to the calculated angle in the y-axis
    //        navmeshAgent.transform.rotation = Quaternion.Euler(0, angle, 0);
    //        transform.rotation = Quaternion.Euler(0, angle, 0);
    //    }
    //    //navmeshAgent.transform.localRotation = Quaternion.identity;
    //}

   
}
