using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class AnimatorDataHandler : AnimatorManager
{
    [Header("References")]
    public bool CanRotate;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        animator.SetFloat("charVelX", horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat("charVelZ", verticalMovement, 0.1f, Time.deltaTime);
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
