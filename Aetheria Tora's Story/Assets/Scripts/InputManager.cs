using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    // W is +1y, S is -1y, A is -1x, D is +1x
    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector2 aim;
    [SerializeField] private bool jump;
    [SerializeField] private bool roll;
    //[SerializeField] private bool sprint;
    [SerializeField] private bool crouch;
    [SerializeField] private bool leftClick;
    [SerializeField] private bool rightClick; 
    [SerializeField] private bool scrollUp;
    [SerializeField] private bool scrollDown;
    private Animator animator;
    [SerializeField]  private Gun gun;
   
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    public bool isInteracting;
    public bool canDoCombo;
    public bool comboFlag;


    [SerializeField] private bool onRollValue;
    [SerializeField] float onRollTimer = 0;

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

       //  ResetBooleans();

        
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

        if (playerInventory.rightWeapon.isMelee)
        {

            animator.SetLayerWeight(animator.GetLayerIndex("Aiming"), 0);

        }
        else
        {

            animator.SetLayerWeight(animator.GetLayerIndex("Aiming"), 1);
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

        if (playerInventory.rightWeapon.isMelee)
        {
            if (canDoCombo)
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
        }
        else
        {
            //gun.Shoot();
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

    public void OnRoll(InputValue value)
    {
        roll = true;
        print("tap");
    }

    public void OnSprint(InputValue value)
    {
        //sprint = true;
        print("hold");
    }
    public void OnResetRoll(InputValue value)
    {
        //sprint = false;
        roll = false;
    }

    private void ResetBooleans()
    {

        if (onRollValue)
        {
            onRollTimer += Time.deltaTime;

            if (onRollTimer > 0 && onRollTimer <= 0.3)
            {
               // sprint = false;
                roll = true;
            }
            else if (onRollTimer > 0.3)
            {
                //sprint = true;
                roll = false;
            }
        }
        else
        {
            roll = false;
           // sprint = false;
            onRollTimer = 0;
        }
       

      

       
    }

    public void OnCrouch(InputValue value)
    {
        crouch = !crouch;
       // CrouchInput(value.isPressed);
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

    private void RollInput(bool value)
    {
        roll = value;
    }

    private void CrouchInput(bool value)
    {
        crouch = value;
    }

    public Vector2 Movement
    {
        get { return movement; }
    }
    public bool Roll
    {
        get { return roll; }
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