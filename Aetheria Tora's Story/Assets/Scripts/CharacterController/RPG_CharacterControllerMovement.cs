using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_CharacterControllerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _camera;

    [Header("Movement")]
    [SerializeField] private Vector3 _inputDirection;
    [SerializeField] private Vector3 _movementDirection;
    [SerializeField] private float _xRotation;

    [Header("Jumping")]
    public float _jumpHeight = 1.5f;
    public bool _enableMidairJumps = false;
    public LayerMask _ignoredLayers;
    public float _groundedTolerance = 0.16f;
    [SerializeField] private bool _jump = false;


    [Header("Sliding")]
    public bool _enableSliding = true;
    public float _slidingTimeout = 0.1f;
    public float _antiStuckTimeout = 0.1f;

    [Header("Falling")]
    public float _fallingThreshold = 6.0f;
    public float _gravity = 20.0f;

    [Header("Speeds")]
    [SerializeField] private float _finalSpeed;
    public float _runSpeed = 6.0f;
    public float _strafeSpeed = 6.0f;
    public float _crouchSpeed = 1.5f;
    public float _midairSpeed = 2.0f;

    public float _sprintSpeedMultiplier = 1.5f;

   

    [Header("Booleans")]
    public bool _moveWithMovingGround = true;
    public bool _rotateWithMovingGround = true;
    public bool _groundAffectsJumping = true;
    public bool _enableCollisionMovement = true;
    [SerializeField] private bool _resetXandZRotations = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }

}
