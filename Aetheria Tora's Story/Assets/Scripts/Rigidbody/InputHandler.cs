using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using JohnStairs.RCC.Character.ARPG;
using JohnStairs.RCC.Inputs;
using UnityEngine.SceneManagement;

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
    public bool key1Flag;
    public bool key2Flag;
    public bool leftClickTapFlag;
    public float rollInputTimer;
    public float leftClickInputTimer;

    public bool crouchingFlag = false;

    public RPGControllerARPG controllerARPG; 
    public Rig rig;
    public RPGInputActions inputActions;
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
        rig = GetComponentInChildren<Rig>();
        controllerARPG = GetComponent<RPGControllerARPG>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = RPGInputManager.GetInputActions();
            inputActions.Character.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Character.RotationAmount.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.Character.Crouch.performed += j => crouchingFlag = !crouchingFlag;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
       // crouchingFlag = inputActions.Character.Crouch.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
       SwitchScenes();
        MoveInput(delta);
        HandleRollInput(delta);
        HandleReload();
        HandleRangedInput(delta);
        //HandleAttackInput(delta);
        //HandleQuickSlotsInput();
        controllerARPG.ActivateCharacterControl = !animatorHandler.animator.GetBool("isInteracting") && !playerManager.isShooting;
    }

    private void SwitchScenes()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;

        if (moveAmount > 0)
            rig.weight = 0;

        //if (horizontal > 0 || vertical > 0)
        //{
        //    playerManager.anim.applyRootMotion = false;
        //}

        if (inputActions.Character.Key1.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
        {
            playerInventory.ChangeRightWeapon(0);
            playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 0);
            rig.weight = 0;
        }
        else if (inputActions.Character.Key2.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
        {
            playerInventory.ChangeRightWeapon(1);
            playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 0);
            rig.weight = 0;
        }
    }

    public void HandleReload()
    {
        reloadInput = inputActions.Character.Reload.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }

    private void HandleRollInput(float delta)
    {
        shiftInput = inputActions.Character.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Performed && playerStats.staminaBar.slider.value > 5;
        rollFlag = (inputActions.Character.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed && playerStats.staminaBar.slider.value > 5) && moveAmount > 0;

        if (shiftInput)
        {
            controllerARPG.ActivateCharacterControl = false;
            sprintFlag = true;
            playerStats.TakeStaminaDamage(1);
        }
    }

    public void HandleRangedInput(float delta)
    {
        if (playerInventory.currentRightWeaponIndex == 0) //0 is the index for the gun 
        {
            leftClickInput = inputActions.Character.MoveForwardHalf1.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            rightClickInput = inputActions.Character.MoveForwardHalf2.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (leftClickInput)
            {
                //set a bool true
                leftClickInputTimer += delta;
                leftClickTapFlag = false;
                playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 1);
                rig.weight = 0.5f;
                playerManager.isShooting = true;
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

        if (playerInventory.currentRightWeaponIndex == 1)
        {
            inputActions.Character.MoveForwardHalf1.performed += i => leftClickInput = true;
            inputActions.Character.MoveForwardHalf2.performed += i => rightClickInput = true;

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
        //inputActions.Character.ScrollUp.performed += i => scrollUp = true;
        //inputActions.Character.ScrollDown.performed += i => scrollDown = true;

        //if (scrollDown)
        //{
        //    playerInventory.ChangeRightWeapon();
        //}
        //else if (scrollUp)
        //{
        //    playerInventory.ChangeRightWeapon();
        //}

        if (playerInventory.rightWeapon.isMelee)
        {
            playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 0);
            rig.weight = 0;

            spearImage.enabled = true;
            pistolImage.enabled = false;
        }
        else
        {
           // playerManager.anim.SetLayerWeight(playerManager.anim.GetLayerIndex("Aiming"), 0);

            spearImage.enabled = false;
            pistolImage.enabled = true;
        }
    }
}
