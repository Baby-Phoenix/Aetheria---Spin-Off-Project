using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    CapsuleCollider playerCollider;
    public PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerCollider = GetComponentInParent<CapsuleCollider>();
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

    public void EnableIsInvulnerable()
    {
        print("enable");
        animator.SetBool("isInvulnerable", true);
    }

    public void DisableIsInvulnerable()
    {
        print("disable");
        animator.SetBool("isInvulnerable", false);
    }

    //public void OnAnimatorMove()
    //{
    //    //if (playerManager.isInteracting == false)
    //    //    return;

    //    //float delta = Time.deltaTime;
    //    //playerLocomotion.rigidbody.drag = 0;
    //    //Vector3 deltaPosition = animator.deltaPosition;
    //    //deltaPosition.y = 0;
    //    //Vector3 velocity = deltaPosition / delta;
    //    //playerLocomotion.rigidbody.velocity = velocity;
    //}

    private void SetColliderHeight(float height)
    {
        playerCollider.height = height;
    }

    private void SetColliderPosY(float yPos)
    {
        playerCollider.center = new Vector3(playerCollider.center.x, yPos, playerCollider.center.z);
    }

    private void playerSwing()
    {
        FindObjectOfType<AudioManager>().Play("swing");
    }
    private void playerGetHit()
    {
        FindObjectOfType<AudioManager>().Play("GetHit");
    }
}


