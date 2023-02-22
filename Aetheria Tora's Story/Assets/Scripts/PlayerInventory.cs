using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem rightWeapon;
    //public WeaponItem leftWeapon;
    //public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    //public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    //public WeaponItem currentRightWeapon;
    public int currentRightWeaponIndex = 0;
    //public int currentLeftWeaponIndex = -1;

    private void Awake()
    {
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    private void Start()
    {
        //rightWeapon = unarmedWeapon;
        //leftWeapon = unarmedWeapon;
        //rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        //leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
        rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        //weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = 0;
            //rightWeapon = unarmedWeapon;
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }
        else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }


        //leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
    }


}