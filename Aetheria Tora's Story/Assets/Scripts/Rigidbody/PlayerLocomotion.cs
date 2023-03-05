using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private Transform aimObjectTransform;
    [SerializeField] private LayerMask ignorePlayerMask;
    PlayerManager playerManager;
    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDirection;


    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Ground & Air Detection Stats")]
    [SerializeField]
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    [Header("Movement Stats")]
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float walkingSpeed = 1;
    [SerializeField]
    float sprintSpeed = 7;
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float fallingSpeed = 45;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        aimRig = GetComponentInChildren<Rig>(); 
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }


    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleAiming(float delta)
    {
        Camera camera = cameraObject.gameObject.GetComponent<Camera>();
        Vector3 mousePosition = new Vector3(inputHandler.mouseX, inputHandler.mouseY, 0);

        Ray ray = camera.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, ignorePlayerMask))
        {
            aimObjectTransform.position = raycastHit.point;
        }
    }

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward;
        //targetDir += cameraObject.right;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = myTransform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
            return;

        if (playerManager.isInteracting)
            return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5 && inputHandler.vertical > 0) 
        {
            
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (inputHandler.moveAmount < 0.5)
            {
                moveDirection *= walkingSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }

        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues("charVelX", "charVelZ", inputHandler.horizontal, inputHandler.vertical, playerManager.isSprinting);


        HandleRotation(delta);
        
    }

    public void HandleJumping()
    {
        if (animatorHandler.animator.GetBool("isInteracting"))
            return;

        if (inputHandler.spaceInput)
        {
            moveDirection = cameraObject.forward;
            animatorHandler.PlayTargetAnimation("Jumping", true);
            moveDirection.y = 0;
            Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
            myTransform.rotation = jumpRotation;
        }
    }
    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.animator.GetBool("isInteracting"))
            return;
       
        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward;
           // moveDirection += cameraObject.right;

            animatorHandler.PlayTargetAnimation("Rolling", true);
            moveDirection.y = 0;
            Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
            myTransform.rotation = rollRotation;
        }
    }

    

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            if (playerManager.isInteracting == false)
            {
                animatorHandler.PlayTargetAnimation("Falling", true);
            }

            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 9f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    animatorHandler.PlayTargetAnimation("Landing", true);
                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (playerManager.isInAir == false)
            {
                

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            myTransform.position = targetPosition;
        }

        //if (playerManager.isGrounded)
        //{
        //    if(playerManager.isInteracting || inputHandler.moveAmount > 0)
        //    {
        //        myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
        //    }
        //    else
        //    {
        //        myTransform.position = targetPosition;
        //    }
        //}

    }

    #endregion
}

//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerLocomotion : MonoBehaviour
//{
//    [SerializeField] Transform cameraObject;
//    [SerializeField] InputHandler inputHandler;
//    [SerializeField] Vector3 moveDirection;

//    [HideInInspector] public Transform myTransform;
//    [HideInInspector] public AnimatorHandler animatorHandler;

//    public Rigidbody rigidBody;
//    public GameObject normalCamera;

//    [Header("Stats")]
//    [SerializeField] float movementSpeed = 5;
//    [SerializeField] float rotationSpeed = 10;

//    void Start()
//    {
//        rigidBody = GetComponent<Rigidbody>();
//        inputHandler = GetComponent<InputHandler>();
//        animatorHandler = GetComponentInChildren<AnimatorHandler>();
//        cameraObject = Camera.main.transform;
//        myTransform = transform;

//    }

//    private void FixedUpdate()
//    {
//        float delta = Time.deltaTime;

//        HandleMovement(delta);
//        HandleRotation(delta);
//        HandleRollingAndSprinting(delta);


//    }
//    public void Update()
//    {
//        float delta = Time.deltaTime;

//        inputHandler.TickInput(delta);
//        animatorHandler.UpdateAnimatorValues("charVelX", "charVelZ", inputHandler.horizontal, inputHandler.vertical, false);



//    }

//    #region Movement
//    Vector3 normalVector;
//    Vector3 targetPosition;

//    private void HandleRotation(float delta)
//    {
//        Vector3 targetDir = Vector3.zero;
//        float moveOverride = inputHandler.moveAmount;

//        targetDir = cameraObject.forward;
//        //targetDir += cameraObject.right;

//        targetDir.Normalize();
//        targetDir.y = 0;

//        if (targetDir == Vector3.zero)
//        {
//            targetDir = myTransform.forward;

//        }

//        float rs = rotationSpeed;

//        Quaternion tr = Quaternion.LookRotation(targetDir);
//        Quaternion targetRotation = Quaternion.Lerp(myTransform.rotation, tr, rs * delta);

//        myTransform.rotation = targetRotation;
//    }

//    private void HandleMovement(float delta)
//    {
//        moveDirection = cameraObject.forward * inputHandler.vertical;
//        moveDirection += cameraObject.right * inputHandler.horizontal;
//        moveDirection.Normalize();
//        moveDirection.y = 0;

//        float speed = movementSpeed;
//        moveDirection *= speed;

//        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
//        rigidBody.velocity = projectedVelocity;

//    }

//    private void HandleRollingAndSprinting(float delta)
//    {
//        if (animatorHandler.animator.GetBool("isInteracting"))
//            return;

//        if (inputHandler.rollFlag)
//        {
//            moveDirection = cameraObject.forward;
//            moveDirection += cameraObject.right;
//            moveDirection.Normalize();

//            animatorHandler.UpdateRollAnimatorValues(inputHandler.horizontal, inputHandler.vertical);

//            animatorHandler.PlayTargetAnimation("Rolling", true);
//            moveDirection.y = 0;
//            Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
//            // myTransform.rotation = rollRotation;

//        }
//    }

//    #endregion
//}

