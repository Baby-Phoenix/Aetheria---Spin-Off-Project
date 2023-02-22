using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class AnimatorDataHandler : AnimatorManager
{
    [Header("References")]
    public bool CanRotate;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimatorValues(string valuex, string valuez, float horizontalMovement, float verticalMovement)
    {
        animator.SetFloat(valuex, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(valuez, verticalMovement, 0.1f, Time.deltaTime);
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
