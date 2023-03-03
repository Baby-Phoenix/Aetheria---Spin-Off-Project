using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool rb_input;
    public bool rt_input;

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public float rollInputTimer;

    InputControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputControls();
            inputActions.Player.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Player.Aim.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        //HandleAttackInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        b_Input = inputActions.Player.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        if (b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    //private void HandleAttackInput(float delta)
    //{
    //    inputActions.Player.RB.performed += i => rb_input = true;
    //    inputActions.Player.RT.performed += i => rt_input = true;

    //    //RB input handles the right hand waepon's light attack
    //    if (rb_input)
    //    {
    //        if (playerManager.canDoCombo)
    //        {
    //            comboFlag = true;
    //            playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
    //            comboFlag = false;
    //        }
    //        else
    //        {
    //            playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
    //        }
    //    }

    //    if (rt_input)
    //    {
    //        playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
    //    }
    //}
}
