using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool reloadInput;

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool leftClickTapFlag;
    public float rollInputTimer;
    public float leftClickInputTimer;

    public InputControls inputActions;
    PlayerAttacker playerAttacker;
    public PlayerInventory playerInventory;
    PlayerManager playerManager;
    PlayerStats playerStats;
    AnimatorHandler animatorHandler;

    public Image pistolImage;
    public Image spearImage;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
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
        HandleReload();
        HandleRangedInput(delta);
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

    public void HandleReload()
    {
        reloadInput = inputActions.Player.Reload.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }

    private void HandleRollInput(float delta)
    {
        shiftInput = inputActions.Player.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed && playerStats.staminaBar.slider.value > 5;
        
        if (shiftInput)
        {
            rollInputTimer += delta;
            sprintFlag = true;
            playerStats.TakeStaminaDamage(1);
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                animatorHandler.EnableIsInvulnerable();
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    public void HandleRangedInput(float delta)
    {
        if (!playerInventory.rightWeapon.isMelee)
        {
            leftClickInput = inputActions.Player.LeftClick.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            rightClickInput = inputActions.Player.RightClick.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (leftClickInput)
            {
                //set a bool true
                leftClickInputTimer += delta;
                leftClickTapFlag = false;
            }
            else
            {
                if (leftClickInputTimer > 0 && leftClickInputTimer < 0.5f)
                {
                    leftClickInput = false;
                    leftClickTapFlag = true;
                }

                leftClickInputTimer = 0;
            }
        }
    }

    public void HandleAttackInput(float delta)
    {

        if (playerInventory.rightWeapon.isMelee)
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

            if (rightClickInput && playerStats.staminaBar.slider.value > 5) 
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

            spearImage.enabled = true;
            pistolImage.enabled = false;
        }
        else
        {
            playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 1);

            spearImage.enabled = false;
            pistolImage.enabled = true;
        }
    }
}
