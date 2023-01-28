using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDataManager : MonoBehaviour
{
    [Header("References")]
    private InputManager inputManager;
    private PlayerMovement playerMovement;
    private Animator animator;
    public LayerMask groundMask;

    [Header("Player Flags")]
    private bool isGrounded;
    private bool isRunning;
    private bool isCrouching;
    private bool isSprinting;
    private bool isJumping;
    private bool canDoCombo;
    private bool hasAnimator;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        hasAnimator = TryGetComponent(out animator);
        playerMovement = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        hasAnimator = TryGetComponent(out animator);
        GroundedChecker();
        InputHandler();
    }

    private void GroundedChecker()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - (-0.14f) - playerMovement.characterController.height / 2.6f,
                            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePos,
                                         playerMovement.characterController.radius, 
                                         groundMask,
                                         QueryTriggerInteraction.Ignore);

        if (hasAnimator)
        {
            animator.SetBool("isGrounded", isGrounded);
        }
    }
    private void InputHandler()
    {
        if (inputManager == null) { return; }

        //Movement Input
        if (isGrounded) 
        {
           

            isCrouching = inputManager.Crouch;
            isJumping = inputManager.Jump;

        }

    }

    public bool Grounded
    {
        get { return isGrounded; }
    }

    public bool Running
    {
        get { return isRunning; }
    }

    public bool Sprinting
    {
        get { return isSprinting; }
    }

    public bool Crouching
    {
        get { return isCrouching;}
    }
    public bool Jumping
    {
        get { return isJumping; }
    }

    public bool Sliding
    {
        get
        {
            if (isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                return Vector3.Angle(slopeHit.normal, Vector3.up) > playerMovement.characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

}
