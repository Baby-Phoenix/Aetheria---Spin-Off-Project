using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class AnimatorDataHandler : AnimatorManager
{
    InputManager inputManager;
    PlayerMovement playerMovement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    

    public void UpdateAnimatorValues(string valuex, string valuez, float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        if(isSprinting)
        {
            verticalMovement *= 2;
            horizontalMovement *= 2;
        }
        animator.SetFloat(valuex, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(valuez, verticalMovement, 0.1f, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        if (!inputManager.isInteracting)
            return;

        float delta = Time.deltaTime;
        playerMovement.rigidBody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerMovement.rigidBody.velocity = velocity;
    }

    public void UpdateRollAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        animator.SetFloat("rollX", horizontalMovement);
        animator.SetFloat("rollZ", verticalMovement);
    }

    public Vector2 GetAnimatorValues(string valuex, string valuez)
    {
        return new Vector2(animator.GetFloat(valuex), animator.GetFloat(valuez));
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }
}
