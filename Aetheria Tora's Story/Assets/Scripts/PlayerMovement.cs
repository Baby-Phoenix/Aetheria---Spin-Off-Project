using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private InputManager inputManager;
    public CharacterController characterController;
    private AnimatorDataHandler animatorDataHandler;
    private GameObject mainCamera;

    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public float slopeSpeed;
    public float speedChangeRate = 10.0f;
    public Vector3 targetMotion;

    [Header("Jumping")]
    public float jumpHeight = 1.2f;
    public float gravity = -15.0f;
    public float jumpTimeout = 0.5f; // Change it to jump Cooldown
    public float verticalVelocity;
    private float terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    [Header("Falling")]
    public bool isGrounded;
    public float fallTimeout = 0.15f; // Change it to fall cooldown
    public float groundedOffset = -0.14f;
    public LayerMask groundLayer;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchHeight = 0.5f;
    private float standingHeight = 2f;

    [Header("Camera")]
    //put camera stuff here if needed
    public float sensX;
    public float sensY;
    private float rotationX;
    private float rotationY;
    public float rotationSmoothTime = 0.12f;

    //player
    // player
    private float speed;
    private float animationBlend;

    private Vector3 hitPointNormal;



    private void Start()
    {
        animatorDataHandler = GetComponentInChildren<AnimatorDataHandler>();
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        GroundedChecker();
       // SpeedHandler();
        JumpHandler();
        //MovementHandler();
        AnimationHandler();
        RollHandler();

    }

    private void LateUpdate() 
    {
        CameraMovement();
    }
    private void RollHandler() 
    {
        if (inputManager.Roll)
        {
            if (!animatorDataHandler.animator.GetBool("isInteracting"))
            {
                animatorDataHandler.UpdateRollAnimatorValues(inputManager.Movement.x, inputManager.Movement.y);
                animatorDataHandler.animator.SetBool("isRoll", true);
                animatorDataHandler.PlayTargetAnimation("Rolling", true);
            }
           
        }
    }

    private void AnimationHandler()
    {
        //moveDirection = transform.InverseTransformDirection(moveDirection);

        animatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", inputManager.Movement.x, inputManager.Movement.y);

        //anim.SetBool("IsJump", isJump);
        //anim.SetBool("IsGround", controller.velocity.y <= 0);
        //anim.SetFloat("Velocity X", moveDirection.x, 0.05f, Time.deltaTime);
        //anim.SetFloat("Velocity Y", controller.velocity.y, 0.05f, Time.deltaTime);
        //anim.SetFloat("Velocity Z", moveDirection.z, 0.05f, Time.deltaTime);
    }

    private void MovementHandler()
    {
        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1;

        if (currentHorizontalSpeed < moveSpeed - speedOffset ||
            currentHorizontalSpeed > moveSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, moveSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
            speed = moveSpeed;

        animationBlend = Mathf.Lerp(animationBlend, moveSpeed, Time.deltaTime * speedChangeRate);

        if (animationBlend < 0.01f)
            animationBlend = 0f;

        //if (hasAnimator)
        //{
        //    anim.SetFloat("Speed", animationBlend);
        //    anim.SetFloat("MotionSpeed", inputMagnitude);
        //}

        Vector2 inputVector = inputManager.Movement.normalized; 

        Vector3 inputDir = new Vector3(inputVector.x, 0, inputVector.y);
        float rotationVelocity = 1f;
        float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float finalRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

        //if can rotate
        if(!animatorDataHandler.animator.GetBool("isInteracting"))
            transform.rotation = Quaternion.Euler(0.0f, finalRotation, 0.0f);


        targetMotion = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        
        //characterController.Move(targetMotion.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void GroundedChecker()
    {
        Vector3 spherePos = new Vector3(transform.position.x, (characterController.center.y + transform.position.y) - groundedOffset - characterController.height / 2.6f,
                            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePos, characterController.radius, groundLayer,
                     QueryTriggerInteraction.Ignore);

        //anim.SetBool("isGrounded", isGrounded);//change this to the new animation handler

    }

    private void JumpHandler()
    {
        if (isGrounded) //while the player is grounded
        {
            fallTimeoutDelta = fallTimeout;

            //if (hasAnimator)
            //{
            //    anim.SetBool("isJumping", false);
            //    anim.SetBool("isFalling", false);
            //}

            if (verticalVelocity < 0.0f)
                verticalVelocity = -2f;

            if (inputManager.Jump && jumpTimeoutDelta <= 0.0f) // we can call an invoke instead of using jumpTimeoutDetla
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                //if (hasAnimator)
                //{
                //    anim.SetBool("isJumping", true);
                //}
            }
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }

        else //while the players is in the air or falling
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                //if (hasAnimator)
                //{
                //    anim.SetBool("isFalling", true);
                //}
            }
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

    }

    private void CameraMovement()
    {
        //rotate the cam and orientation
        if (!animatorDataHandler.animator.GetBool("isInteracting"))
        {
            // if the angle between player and camera is greater than 10 lerp rotation of player, otherwise dont lerp
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0)) > 10)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0), Time.deltaTime * 10);
            else
                transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 spherePos = new Vector3(transform.position.x, (characterController.center.y + transform.position.y) - groundedOffset - characterController.height / 2.6f,
                             transform.position.z);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(spherePos, characterController.radius);
    }
}
