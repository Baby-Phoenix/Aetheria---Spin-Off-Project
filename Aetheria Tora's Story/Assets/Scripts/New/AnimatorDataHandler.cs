using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class AnimatorDataHandler : MonoBehaviour
{
    [Header("Refernces")]
    private PlayerMovement playerMovement;
    private Animator animator;
    public bool CanRotate;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        animator.SetFloat("X", verticalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat("Z", horizontalMovement, 0.1f, Time.deltaTime);
    }
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }


}
