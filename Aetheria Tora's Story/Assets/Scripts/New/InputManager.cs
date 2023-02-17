using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    // W is +1y, S is -1y, A is -1x, D is +1x
    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector2 aim;
    [SerializeField] private bool jump;
    [SerializeField] private bool sprint;
    [SerializeField] private bool crouch;
    [SerializeField] private bool leftClick;
    [SerializeField] private bool rightClick; 
    [SerializeField] private bool scrollUp;
    [SerializeField] private bool scrollDown;
    [SerializeField] private bool slide;
    private Animator animator;

    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    public bool isInteracting;
    public bool canDoCombo;
    public bool comboFlag;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");

        HandleQuickSlotsInput();
    }
    private void HandleQuickSlotsInput()
    {
        if (scrollDown)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if(scrollUp)
        {
            playerInventory.ChangeRightWeapon();
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnMovement(InputValue value)
    {
        MovementInput(value.Get<Vector2>());
    }

    public void OnAim(InputValue value)
    {
        AimtInput(value.Get<Vector2>());
    }

    public void OnLeftClick(InputValue value)
    {

        if(canDoCombo)
        {
            comboFlag = true;
            playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
            comboFlag = false;
        }
        else
        {
            if (isInteracting)
                return;
            if (canDoCombo)
                return;

            playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
        }


        LeftClickInput(value.isPressed);
    }

    public void OnRightClick(InputValue value)
    {
        if (isInteracting)
            return;

        playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        RightClickInput(value.isPressed);
    }

    public void OnScrollUp(InputValue value)
    {
        ScrollUpInput(value.isPressed);
    }
    public void OnScrollDown(InputValue value)
    {
        ScrollDownInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnCrouch(InputValue value)
    {
        CrouchInput(value.isPressed);
    }

    public void OnSlide(InputValue value)
    {
        SlideInput(value.isPressed);
    }

    private void MovementInput(Vector2 value)
    {
        movement = value;
    }

    private void JumpInput(bool value)
    {
        jump = value;
    }

    private void AimtInput(Vector2 value)
    {
        aim = value;
    }

    private void LeftClickInput(bool value)
    {
        leftClick = value;
    }

    private void RightClickInput(bool value)
    {
        rightClick = value;
    }
    

    private void ScrollUpInput(bool value)
    {
        scrollUp = value;
    }
    private void ScrollDownInput(bool value)
    {
        scrollDown = value;
    }

    private void SprintInput(bool value)
    {
        sprint = value;
    }

    private void CrouchInput(bool value)
    {
        crouch = value;
    }

    private void SlideInput(bool value)
    {
        slide = value;
    }


    public Vector2 Movement
    {
        get
        {
            if (sprint)
            {
                movement = movement.normalized;
                movement *= 2;
            }
            return movement;
        }
    }
    public bool Sprint
    {
        get { return sprint; }
    }
    public Vector2 Aim
    {
        get { return aim; }
    }
    public bool Jump
    {
        get { return jump; }
    }
    public bool Crouch
    {
        get { return crouch; }
    }
    public bool Slide
    {
        get { return slide; }
    }
    public bool LeftClick
    {
        get { return leftClick; }
    }
    public bool RightClick
    {
        get { return rightClick; }
    }
    public bool ScrollUp
    {
        get { return scrollUp; }
    }
    public bool ScrollDown
    {
        get { return scrollDown; }
    }
}