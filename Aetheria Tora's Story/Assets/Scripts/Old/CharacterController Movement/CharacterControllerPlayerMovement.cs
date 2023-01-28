using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerPlayerMovement : MonoBehaviour
{
    [Header("Refernces")]
    private InputManager inputManager;
    public CharacterController characterController;
    private Animator anim;
    private GameObject mainCamera;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
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
    public float fallTimeout = 0.15f; // Change it to fall cooldown
    public bool isGrounded = true;
    public float groundedOffset = -0.14f;
    public LayerMask groundLayer;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchHeight = 0.5f;
    private float standingHeight = 2f;
    private float timeToCrouch = 1f;
    private Vector3 crouchingCenter = new Vector3(0f, 0.5f, 0f);
    private Vector3 standingCenter = Vector3.zero;
    public bool isCrouching = false;
    private bool duringCrouchAnimation = false;
    public bool isToggle = true;

    [Header("Camera")]
    //put camera stuff here if needed
    public float sensX;
    public float sensY;
    private float rotationX;
    private float rotationY;
    public Transform orientation;
    public float rotationSmoothTime = 0.12f;

    //player
    // player
    private float speed;
    private float animationBlend;
   
    private bool hasAnimator;

    private Vector3 hitPointNormal;
    private bool isSliding
    {
        get
        {
            if(isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Audio")]
    float tempHolder;
    //// **STUFF FOR AUDIO CAN GO HERE****

    private void Start()
    {
        hasAnimator = TryGetComponent(out anim);
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        orientation = GameObject.FindWithTag("Orientation").transform;
        mainCamera = GameObject.FindWithTag("MainCamera");

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    private void Update()
    {
        hasAnimator = TryGetComponent(out anim);
        GroundedChecker();
        StateHandler();
        Jumping();
        Move();
        
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

  
    private void GroundedChecker()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - groundedOffset - characterController.height / 2.6f,
                            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePos, characterController.radius, groundLayer, 
                     QueryTriggerInteraction.Ignore);

        if (hasAnimator)
        {
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    private void StateHandler()
    {

        if (isGrounded && inputManager.Movement != Vector2.zero && !inputManager.Sprint) 
        { 
            moveSpeed = walkSpeed;
        }

        else if (isGrounded && inputManager.Movement != Vector2.zero && inputManager.Sprint)
        { 
            moveSpeed = sprintSpeed;
        }


        if (inputManager.Crouch)
        {
            moveSpeed = crouchSpeed;

            if (!duringCrouchAnimation && isToggle)
                StartCoroutine(ToggleCrouchStand());
            else if (!duringCrouchAnimation && !isToggle)
            {
                StartCoroutine(HoldCrouchStand());
            }
        }
        else if (!inputManager.Crouch)
        {
            
            if (!isToggle)
            {
                if (!duringCrouchAnimation)
                {
                    StartCoroutine(HoldCrouchStand());
                }
            }
        }

        if (isGrounded && inputManager.Movement == Vector2.zero)
        {
            moveSpeed = 0.0f;
        }

    }


    private void Jumping()
    {
        if (isGrounded) //while the player is grounded
        {
            fallTimeoutDelta = fallTimeout;

            if (hasAnimator)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
            }

            if (verticalVelocity < 0.0f) 
                verticalVelocity = -2f;

            if (inputManager.Jump && jumpTimeoutDelta <= 0.0f) // we can call an invoke instead of using jumpTimeoutDetla
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (hasAnimator)
                {
                    anim.SetBool("isJumping", true);
                }
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
                if (hasAnimator)
                {
                    anim.SetBool("isFalling", true);
                }
            }
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Move()
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

        if (hasAnimator)
        {
            anim.SetFloat("Speed", animationBlend);
            anim.SetFloat("MotionSpeed", inputMagnitude);
        }

        Vector3 inputDir = new Vector3(inputManager.Movement.x, 0, inputManager.Movement.y);
        float rotationVelocity = 0f;
        float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float finalRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, finalRotation, 0.0f);


        targetMotion = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        if (isSliding)
        {
            targetMotion = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z);
            speed = slopeSpeed;
        }

        characterController.Move(targetMotion.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

    }


    private void CameraMovement()
    {
        float mouseX = inputManager.Aim.x * Time.deltaTime * sensX;
        float mouseY = inputManager.Aim.y * Time.deltaTime * sensY;

        rotationY += mouseX;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        //rotate the cam and orientation
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
        orientation.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    private IEnumerator ToggleCrouchStand()
    {
        duringCrouchAnimation = true;

            float timeElasped = 0;
            float targetHeight = isCrouching ? standingHeight : crouchHeight;
            float currentHeight = characterController.height;
            Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
            Vector3 currentCenter = characterController.center;
            float currentGroundOffset = groundedOffset;
            float targetGroundOffset = isCrouching ? -0.14f : -0.7f;

            while (timeElasped <= timeToCrouch)
            {
                characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElasped / timeToCrouch);
                characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElasped / timeToCrouch);
                groundedOffset = Mathf.Lerp(currentGroundOffset, targetGroundOffset, timeElasped / timeToCrouch);
                timeElasped += Time.deltaTime;
                yield return null;
            }

            isCrouching = !isCrouching;
       
        duringCrouchAnimation = false;
    }
    private IEnumerator HoldCrouchStand()
    {
        duringCrouchAnimation = true;

        bool crouching = inputManager.Crouch;

        float timeElasped = 0;
        float targetHeight = !crouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = !crouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;
        float currentGroundOffset = groundedOffset;
        float targetGroundOffset = !crouching ? -0.14f : -0.7f;

        while (timeElasped <= timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElasped / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElasped / timeToCrouch);
            groundedOffset = Mathf.Lerp(currentGroundOffset, targetGroundOffset, timeElasped / timeToCrouch);
            timeElasped += Time.deltaTime;
            yield return null;
        }


        duringCrouchAnimation = false;
    }

   
}