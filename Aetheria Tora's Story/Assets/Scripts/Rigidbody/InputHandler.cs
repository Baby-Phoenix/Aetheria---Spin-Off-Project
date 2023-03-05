using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool spaceInput;
    public bool shiftInput;
    public bool leftClickInput;
    public bool rightClickInput;
    public bool scrollUp;
    public bool scrollDown;

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
        HandleJumpInput();
        //HandleAttackInput(delta);
        //HandleQuickSlotsInput();
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    public void HandleJumpInput()
    {
        spaceInput = inputActions.Player.Jump.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }

    private void HandleRollInput(float delta)
    {
        shiftInput = inputActions.Player.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        
        if (shiftInput)
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

    public void HandleAttackInput(float delta)
    {
        if(playerInventory.rightWeapon.isMelee)
        {
            inputActions.Player.LeftClick.performed += i => leftClickInput = true;
            inputActions.Player.RightClick.performed += i => rightClickInput = true;

            //RB input handles the right hand weapon's light attack
            if (leftClickInput)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if (rightClickInput)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
        
    }

    public void HandleQuickSlotsInput()
    {
        inputActions.Player.ScrollUp.performed += i => scrollUp = true;
        inputActions.Player.ScrollDown.performed += i => scrollDown = true;

        if (scrollDown)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if (scrollUp)
        {
            playerInventory.ChangeRightWeapon();
        }

        if (playerInventory.rightWeapon.isMelee)
        {

            playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 0);

        }
        else
        {

            playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 1);
        }
    }
}
