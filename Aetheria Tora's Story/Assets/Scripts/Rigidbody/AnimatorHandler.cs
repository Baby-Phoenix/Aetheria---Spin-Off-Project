using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("charVelZ");
        horizontal = Animator.StringToHash("charVelX");
    }

    public void UpdateAnimatorValues(string valuex, string valuez, float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        if (isSprinting)
        {
            verticalMovement *= 2;
            horizontalMovement *= 2;
        }
        animator.SetFloat(valuex, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(valuez, verticalMovement, 0.1f, Time.deltaTime);
    }

    //public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    //{
    //    #region Vertical
    //    float v = 0;

    //    if (verticalMovement > 0 && verticalMovement < 0.55f)
    //    {
    //        v = 0.5f;
    //    }
    //    else if (verticalMovement > 0.55f)
    //    {
    //        v = 1;
    //    }
    //    else if (verticalMovement < 0 && verticalMovement > -0.55f)
    //    {
    //        v = -0.5f;
    //    }
    //    else if (verticalMovement < -0.55f)
    //    {
    //        v = -1;
    //    }
    //    else
    //    {
    //        v = 0;
    //    }

    //    #endregion

    //    #region Horizontal
    //    float h = 0;

    //    if (horizontalMovement > 0 && horizontalMovement < 0.55f)
    //    {
    //        h = 0.5f;
    //    }
    //    else if (horizontalMovement > 0.55f)
    //    {
    //        h = 1;
    //    }
    //    else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
    //    {
    //        h = -0.5f;
    //    }
    //    else if (horizontalMovement < -0.55f)
    //    {
    //        h = -1;
    //    }
    //    else
    //    {
    //        h = 0;
    //    }

    //    #endregion

    //    if (isSprinting)
    //    {
    //        v = 2;
    //        h = horizontalMovement;
    //    }

    //    animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
    //    animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    //}

    //public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    //{
    //    anim.applyRootMotion = isInteracting;
    //    anim.SetBool("isInteracting", isInteracting);
    //    anim.CrossFade(targetAnim, 0.2f);
    //}

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
            return;

        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AnimatorHandler : AnimatorManager
//{
//    [SerializeField] InputHandler inputHandler;
//    [SerializeField] PlayerLocomotion playerLocomotion;

//    private void Start()
//    {
//        animator = GetComponent<Animator>();
//        inputHandler = GetComponentInParent<InputHandler>();
//        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
//    }



//    public void UpdateAnimatorValues(string valuex, string valuez, float horizontalMovement, float verticalMovement, bool isSprinting)
//    {
//        if (isSprinting)
//        {
//            verticalMovement *= 2;
//            horizontalMovement *= 2;
//        }
//        animator.SetFloat(valuex, horizontalMovement, 0.1f, Time.deltaTime);
//        animator.SetFloat(valuez, verticalMovement, 0.1f, Time.deltaTime);
//    }

//    private void OnAnimatorMove()
//    {
//        if (!inputHandler.isInteracting)
//            return;

//        float delta = Time.deltaTime;
//        playerLocomotion.rigidBody.drag = 0;
//        Vector3 deltaPosition = animator.deltaPosition;
//        deltaPosition.y = 0;
//        Vector3 velocity = deltaPosition / delta;
//        playerLocomotion.rigidBody.velocity = velocity;
//    }

//    public void UpdateRollAnimatorValues(float horizontalMovement, float verticalMovement)
//    {
//        animator.SetFloat("rollX", horizontalMovement);
//        animator.SetFloat("rollZ", verticalMovement);
//    }

//    public Vector2 GetAnimatorValues(string valuex, string valuez)
//    {
//        return new Vector2(animator.GetFloat(valuex), animator.GetFloat(valuez));
//    }

//    public void EnableCombo()
//    {
//        animator.SetBool("canDoCombo", true);
//    }

//    public void DisableCombo()
//    {
//        animator.SetBool("canDoCombo", false);
//    }
//}
