using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    public Animator anim;
    public Transform colliderCenter;
    public AnimatorHandler animatorHandler;

    public bool isInteracting;

    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isInvulnerable;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animatorHandler = GetComponent<AnimatorHandler>();
        animatorHandler.Initialize();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isSprinting = inputHandler.shiftInput;
        inputHandler.HandleAttackInput(delta);
        inputHandler.HandleQuickSlotsInput();
    }

    void FixedUpdate()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isInvulnerable = anim.GetBool("isInvulnerable");
        anim.SetBool("isInAir", isInAir);

        inputHandler.TickInput(delta);

        if (inputHandler.rollFlag)
        {
            animatorHandler.PlayTargetAnimation("Rolling", true);
            animatorHandler.animator.SetBool("canMove", false);
            print("rolling is called");

        }
    }
    private void LateUpdate()
    {
        inputHandler.spaceInput = false;
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.leftClickInput = false;
        inputHandler.rightClickInput = false;
        inputHandler.scrollUp = false;
        inputHandler.scrollDown = false;
        inputHandler.leftClickTapFlag = false;
        inputHandler.controllerARPG.ActivateCharacterControl = true;
    }
}
