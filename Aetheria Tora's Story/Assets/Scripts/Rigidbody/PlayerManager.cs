using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    public Animator anim;
    PlayerLocomotion playerLocomotion;

    public bool isInteracting;

    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isSprinting = inputHandler.shiftInput;

        inputHandler.HandleAttackInput(delta);
    }

    void FixedUpdate()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        anim.SetBool("isInAir", isInAir);

        inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);

        
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleJumping();
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

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
}
