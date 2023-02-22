using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : AnimatorManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    AnimatorDataHandler enemyAnimatorDataHandler;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLocomotionManager =  GetComponent<EnemyLocomotionManager>();
        enemyAnimatorDataHandler = GetComponent<AnimatorDataHandler>();
    }

    //private void OnAnimatorMove()
    //{
    //    float delta = Time.deltaTime;
    //    Vector3 deltaPosition = animator.deltaPosition;
    //    deltaPosition.y = 0;
    //    Vector3 velocity = deltaPosition / delta;
    //    //enemyLocomotionManager.enemyCharacterController.Move(velocity);
    //    //enemyAnimatorDataHandler.UpdateAnimatorValues(velocity.x, velocity.z);
    //}
}
