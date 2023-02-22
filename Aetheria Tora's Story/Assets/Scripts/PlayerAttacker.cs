using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorDataHandler animatorDataHandler;
    InputManager inputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    InputActionAsset asset;
    

    private void Awake()
    {
        animatorDataHandler = GetComponent<AnimatorDataHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputManager = GetComponent<InputManager>();
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputManager.comboFlag)
        {
            animatorDataHandler.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorDataHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                lastAttack = weapon.OH_Light_Attack_2;
            }
            else if (lastAttack == weapon.OH_Light_Attack_2)
            {
                animatorDataHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                lastAttack = weapon.OH_Light_Attack_2;
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (animatorDataHandler.animator.GetBool("isInteracting") == true) return;

        animatorDataHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (animatorDataHandler.animator.GetBool("isInteracting") == true) return;

        animatorDataHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
    }
}
