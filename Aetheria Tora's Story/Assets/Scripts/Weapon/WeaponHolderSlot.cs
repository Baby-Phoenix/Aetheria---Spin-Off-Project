using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride; //Where weapon is instantiated
    public bool isLeftHandSlot;
    public bool isRightHandSlot;
    public bool isInstantiated = false;

    public GameObject currentWeaponModel;
    public GameObject pistolModel;
    public GameObject spearModel;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        if (model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }

    public void LoadWeapon(int index)
    {
        if (!isInstantiated)
        {
            GameObject modelGun = Instantiate(pistolModel) as GameObject;
            if (modelGun != null)
            {
                if (parentOverride != null)
                {
                    modelGun.transform.parent = parentOverride;
                }
                else
                {
                    modelGun.transform.parent = transform;
                }

                modelGun.transform.localPosition = Vector3.zero;
                modelGun.transform.localRotation = Quaternion.identity;
                modelGun.transform.localScale = Vector3.one;
            }

            pistolModel = modelGun;

            GameObject modelSpear = Instantiate(spearModel) as GameObject;
            if (modelSpear != null)
            {
                if (parentOverride != null)
                {
                    modelSpear.transform.parent = parentOverride;
                }
                else
                {
                    modelSpear.transform.parent = transform;
                }

                modelSpear.transform.localPosition = Vector3.zero;
                modelSpear.transform.localRotation = Quaternion.identity;
                modelSpear.transform.localScale = Vector3.one;
            }

            spearModel = modelSpear;
            isInstantiated = true;
        }

        else
        {
            if (index == 0) //This is the index for the gun
            {
                GameObject model = pistolModel;

                if (model != null)
                {
                    if (parentOverride != null)
                    {
                        model.transform.parent = parentOverride;
                    }
                    else
                    {
                        model.transform.parent = transform;
                    }

                    model.transform.localPosition = Vector3.zero;
                    model.transform.localRotation = Quaternion.identity;
                    model.transform.localScale = Vector3.one;
                }


                pistolModel.SetActive(true);
                spearModel.SetActive(false);
            }
            else // This is the index for the spear
            {
                GameObject model = spearModel;

                if (model != null)
                {
                    if (parentOverride != null)
                    {
                        model.transform.parent = parentOverride;
                    }
                    else
                    {
                        model.transform.parent = transform;
                    }

                    model.transform.localPosition = Vector3.zero;
                    model.transform.localRotation = Quaternion.identity;
                    model.transform.localScale = Vector3.one;
                }


                pistolModel.SetActive(false);
                spearModel.SetActive(true);
            }
        }
    }
}