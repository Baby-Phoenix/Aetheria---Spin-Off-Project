using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TwinStickMovement : MonoBehaviour
{
    public Transform firePoint;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadZone = 0.11f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] private Transform root;
    [SerializeField] private LayerMask groundMask;

    public Animator anim;
    float velocityZ = 0.0f, velocityX = 0.0f;

    [SerializeField] private bool isGamepad;

    private Camera mainCamera;
    private CharacterController controller;


    private bool isJump = false;
    public float jumpForce = 4;

    [SerializeField] private float jump;
    private Vector2 movement;
    private Vector2 aim;

    private Vector3 playerVelocity;

    private PlayerControls playerControls;
    private PlayerInput playerInput;

   [SerializeField] public static bool isTopDown = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    void HandleInput()
    {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();

        jump = playerControls.Controls.Jump.ReadValue<float>()*4;
        isJump = jump > 0 && !isJump;

    }

    public bool TopDown()
    {
        return isTopDown;
    }

    public bool isGrounded()
    {
        //the player's center is different from the actual center thus messing up the raycast calculation
        bool ground = Physics.Raycast(root.position, -transform.up, 0.01f, groundMask);
        anim.SetBool("IsGround", ground);
        return ground;
    }

    void HandleMovement()
    {
        Vector3 move;
        if (isTopDown)
        {
            move = new Vector3(movement.x, 0, movement.y);
        }
        else
        {
            move = new Vector3(movement.x, jump, 0);
        }

        controller.Move(move * Time.deltaTime * playerSpeed);
        GetComponent<PlayerSideScrollAim>().mousePos = aim;

        HandleAnimation();
    }

    void HandleAnimation()
    {

        if (!isGrounded())
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
     

        Vector3 moveDirection = new Vector3(movement.x, 0, movement.y);

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection = moveDirection.normalized;
        }

        moveDirection = transform.InverseTransformDirection(moveDirection);

        anim.SetBool("IsJump", isJump);
        anim.SetBool("IsGround", controller.velocity.y <= 0);
        anim.SetFloat("Velocity X", moveDirection.x, 0.05f, Time.deltaTime);
        anim.SetFloat("Velocity Y", controller.velocity.y, 0.05f, Time.deltaTime);
        anim.SetFloat("Velocity Z", moveDirection.z, 0.05f, Time.deltaTime);

    }

    void HandleRotation()
    {
        if (isGamepad)
        {
            if (Mathf.Abs(aim.x) > controllerDeadZone || Mathf.Abs(aim.y) > controllerDeadZone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, gamepadRotateSmoothing * Time.deltaTime);
                }
            }
        }
        else
        {
            //Point on screen from ray
            if (isTopDown)
            {
                //Ray ray = Camera.main.ScreenPointToRay(aim);
                //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                //float rayDistance;

                //if (groundPlane.Raycast(ray, out rayDistance))
                //{
                //    Vector3 point = ray.GetPoint(rayDistance);

                //    LookAt(point);
                //}

                var (success, position) = GetMousePosition();
                if (success)
                {
                    //Calculate the direction
                    var direction = position - transform.position;

                    direction.y = 0;

                    //Make transform look in the direction
                    transform.forward = direction;
                    firePoint.forward = direction;
                }
            }
            else
            {
                Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);

                Vector3 heightCorrectedPoint = new Vector3(Camera.main.ScreenToWorldPoint(mouseScreenPosition).x, transform.position.y, transform.position.z);

                transform.LookAt(heightCorrectedPoint, Vector3.up);
            }
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(aim);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            //Raycast hit something and returns position
            return (success: true, position: hitInfo.point);
        }
        else
        {
            //Raycast did not hit anything
            return (success: false, position: Vector3.zero);
        }
    }

    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    private void OnDrawGizmos()
    {
        Vector3 to = new Vector3(transform.position.x, transform.position.y - .01f, transform.position.z);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, to);
    }

    public Vector2 GetInput()
    {
        return movement;
    }
}
