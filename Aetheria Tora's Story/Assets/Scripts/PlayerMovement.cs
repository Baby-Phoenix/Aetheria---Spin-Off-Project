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
   [SerializeField] private AnimatorDataHandler animatorDataHandler;
    private GameObject mainCamera;
    public Transform lookat;
    public new Rigidbody rigidBody;

    [Header("Movement")]
    public float moveSpeed = 5;
    public float runSpeed;
    public float sprintSpeed;
    public float slopeSpeed;
    public float speedChangeRate = 10.0f;
    public Vector3 targetMotion;
    Vector3 normalVector;
    Vector3 moveDirection;

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
    public bool isToggle = true;

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
        animatorDataHandler = GetComponent<AnimatorDataHandler>();
        inputManager = GetComponent<InputManager>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        

        GroundedChecker();
       // SpeedHandler();
        JumpHandler();
        MovementHandler(delta);
        AnimationHandler();
        RollHandler(delta);
        CrouchHandler();

    }

    private void LateUpdate() 
    {
        CameraMovement();
    }

    private void CrouchHandler()
    {
        if (!animatorDataHandler.animator.GetBool("isInteracting"))
        {
            animatorDataHandler.animator.SetBool("isCrouching", inputManager.Crouch);
        }
    }

    private void RollHandler(float delta) 
    {
        if (!animatorDataHandler.animator.GetBool("isInteracting"))
        {
            if (inputManager.Roll)
            {
                moveDirection = mainCamera.transform.forward * inputManager.Movement.y;
                moveDirection += mainCamera.transform.right * inputManager.Movement.x;

                animatorDataHandler.UpdateRollAnimatorValues(inputManager.Movement.x, inputManager.Movement.y);
                animatorDataHandler.PlayTargetAnimation("Rolling", true);

                moveDirection.y = 0;
                //Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                //transform.rotation = rollRotation;

            }
        }
    }

    private void AnimationHandler()
    {
        //moveDirection = transform.InverseTransformDirection(moveDirection);

        animatorDataHandler.UpdateAnimatorValues("charVelX", "charVelZ", inputManager.Movement.x, inputManager.Movement.y, false);

        //anim.SetBool("IsJump", isJump);
        //anim.SetBool("IsGround", controller.velocity.y <= 0);
        //anim.SetFloat("Velocity X", moveDirection.x, 0.05f, Time.deltaTime);
        //anim.SetFloat("Velocity Y", controller.velocity.y, 0.05f, Time.deltaTime);
        //anim.SetFloat("Velocity Z", moveDirection.z, 0.05f, Time.deltaTime);
    }

    private void MovementHandler( float delta)
    {
        moveDirection = mainCamera.transform.forward * inputManager.Movement.y;
        moveDirection += mainCamera.transform.right * inputManager.Movement.x;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = moveSpeed;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidBody.velocity = projectedVelocity;
    }

    private void GroundedChecker()
    {
      

    }

    private void JumpHandler()
    {
        if (isGrounded)
        {
            if (inputManager.Jump)
            {
                if (!animatorDataHandler.animator.GetBool("isInteracting"))
                {
                     animatorDataHandler.UpdateRollAnimatorValues(inputManager.Movement.x, inputManager.Movement.y);
                    animatorDataHandler.PlayTargetAnimation("Jumping", true);
                }
            }
        }

    }

    private void CameraMovement()
    {
        lookat.position = transform.position;
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0)) > 10)
            lookat.rotation = Quaternion.Lerp(lookat.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0), Time.deltaTime * 10);
        else
            lookat.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);

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
    //private void OnDrawGizmos()
    //{
    //    Vector3 spherePos = new Vector3(transform.position.x, (characterController.center.y + transform.position.y) - groundedOffset - characterController.height / 2f,
    //                        transform.position.z);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(spherePos, characterController.radius);
    //}
}
