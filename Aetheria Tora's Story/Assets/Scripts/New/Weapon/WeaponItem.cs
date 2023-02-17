using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string Right_Arm_Idle_01;


    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Light_Attack_3;
    public string OH_Heavy_Attack_1;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
}

