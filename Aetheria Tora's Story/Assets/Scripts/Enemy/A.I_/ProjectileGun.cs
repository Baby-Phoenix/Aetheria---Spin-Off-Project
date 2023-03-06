using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    public GameObject bullet;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    bool isShooting, isReadyToShoot, isReloading;

    public Transform attackPoint;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        isReadyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {

    }
}
