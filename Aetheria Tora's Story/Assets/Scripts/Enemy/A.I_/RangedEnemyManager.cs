using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyManager : MonoBehaviour
{
    RangedEnemyLocomotionManager rangedEnemyLocomotionManager;
    public bool isPerformingAction;

    [Header("A.E Settings")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    private void Awake()
    {
        rangedEnemyLocomotionManager = GetComponent<RangedEnemyLocomotionManager>();
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if(rangedEnemyLocomotionManager == null)
        {
            rangedEnemyLocomotionManager.HandleDetection();
        }
        else
        {
            rangedEnemyLocomotionManager.HandleMoveToTarget();
        }
    }
}
