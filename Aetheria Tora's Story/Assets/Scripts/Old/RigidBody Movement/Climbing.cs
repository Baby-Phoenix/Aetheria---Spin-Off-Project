using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;
    RigidBodyPlayerMovement playerMove;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("ClimbJumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;
    
    public int climbJumps;
    private int climbJumpLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private InputManager inputManager;
    private float horizontalInput;
    private float verticalInput;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;
    

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerMove = GetComponent<RigidBodyPlayerMovement>();
    }

    private void Update()
    {
        MyInput();
        WallCheck();
        StateMachine();

        if (climbing && !exitingWall) ClimbingMovement();
    }

    private void MyInput()
    {
        horizontalInput = inputManager.Movement.x;
        verticalInput = inputManager.Movement.y;
    }
    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || playerMove.grounded) 
        {
            climbTimer = maxClimbTime;
            climbJumpLeft = climbJumps;
        }
    }

    private void StateMachine()
    {
        //State 1 - Climbing
        if (wallFront && verticalInput == 1 && wallLookAngle < maxWallLookAngle && !exitingWall) 
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }

        else if (exitingWall)
        {
            if (climbing) StopClimbing();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer < 0) exitingWall = false;
        }

        else
        {
            if (climbing) StopClimbing();
        }

        if (wallFront && inputManager.Jump && climbJumpLeft > 0) ClimbJump(); //try to put state machine in fixed update due to climbjump being a force;
    }

  

    private void StartClimbing()
    {
        climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;

        //camera fov change
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    //play sound effect while climbing
    }

    private void StopClimbing()
    {
        climbing = false;

        //particle effect once the climb timer is done and the character falls
    }
    
    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        climbJumpLeft--;
    }

}
