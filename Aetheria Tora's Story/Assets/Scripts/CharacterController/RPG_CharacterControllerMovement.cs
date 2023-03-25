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
    [SerializeField] private float _midairJumpsCount = 0;

    [Header("Sliding")]
    public bool _enableSliding = true;
    public float _slidingTimeout = 0.1f;
    public float _antiStuckTimeout = 0.1f;
    [SerializeField] private float _slidingBuffer;

    [Header("Falling")]
    public float _fallingThreshold = 6.0f;
    public float _gravity = 20.0f;
    [SerializeField] private Sphere _colliderBottom;
    [SerializeField] private float _colliderBottomOffset;
    [SerializeField] private bool _grounded;

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
    [SerializeField] private bool _crouching = false;
    [SerializeField] private bool _walking = false;
    [SerializeField] private bool _sprinting = false;
    [SerializeField] private bool _sliding = false;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _canRotate = true;
    [SerializeField] private bool _antiStuckEnabled;

    private struct Sphere
    {
        public Vector3 center;
        public float radius;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    private void Start()
    {
        UpdateColliderBottom();
        _colliderBottomOffset = (_colliderBottom.center + Vector3.down * _colliderBottom.radius).y - transform.position.y;
    }

    private void FixedUpdate()
    {
        if(_resetXandZRotations && _grounded)
        {
            _resetXandZRotations = false;
            ResetXandZrotations();
        }

        if (_enableCollisionMovement)
        {
            ApplyCollisionMovement();
        }
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    /// <summary>
    /// Starts motor computations based on external input
    /// </summary>
    public virtual void StartMotor()
    {
        _canMove = true;
        _canRotate = true;

        if (_slidingBuffer < -_antiStuckTimeout && _characterController.velocity.magnitude < 0.05f)
        {
            _antiStuckEnabled = true;
        }

        if (_antiStuckEnabled)
        {
            _grounded = true;
        }
        else if (_movementDirection.y < 0)
        {
            // Check for grounded only if we are drawn towards the ground
            _grounded = Physics.CheckSphere(_colliderBottom.center + Vector3.down * _groundedTolerance, _colliderBottom.radius, ~_ignoredLayers, QueryTriggerInteraction.Ignore);
        }
        else
        {
            // Upward movement, e.g. jumping => disable grounded check so that colliders from the side cannot interrupt the movement
            _grounded = false;
        }

        if (!_grounded)
        {
            _resetXandZRotations = true;
        }

        //_enable3dMovement = IsSwimming() || IsFlying();

        if (_animator.GetBool("Jump"))
        {
            _animator.ResetTrigger("Jump");
        }
    }

    private void ApplyCollisionMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        Vector3 start = _colliderBottom.center;
        Vector3 end = start + Vector3.up * (_characterController.height - _characterController.radius);
        Collider[] colliders = Physics.OverlapCapsule(start, end, _characterController.radius, ~_ignoredLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider c in colliders)
        {
            if (c is MeshCollider || c is TerrainCollider)
            {
                // Mesh colliders do not support the "ClosestPoint" method => skip
                continue;
            }

            // Get the point which is closest to the character, i.e. deepest inside the character collider
            Vector3 closestPoint = c.ClosestPoint(transform.position);
            // Project the closest point to the same height as the character
            closestPoint.y = transform.position.y;
            Vector3 closestPointDirection = closestPoint - transform.position;
            // Add delta movement needed to move the character collider out of collider c 
            moveDirection += closestPointDirection.normalized * (closestPointDirection.magnitude - _characterController.radius);
        }

        if (moveDirection != Vector3.zero)
        {
            // Apply the combined delta movements
            Translate(moveDirection);
        }
    }

    private void UpdateColliderBottom()
    {
        _colliderBottom.center = transform.TransformPoint(_characterController.center) + Vector3.down * (_characterController.height * 0.5f - _characterController.radius);
        _colliderBottom.radius = _characterController.radius;
    }

    private void ResetXandZrotations()
    {
        Vector3 delta = _colliderBottom.center + Vector3.down * (_colliderBottom.radius + _colliderBottomOffset) - transform.position;
        Translate(delta);

    }

    private void Translate(Vector3 translation)
    {
        transform.position += translation;
        UpdateColliderBottom();
    }

}
