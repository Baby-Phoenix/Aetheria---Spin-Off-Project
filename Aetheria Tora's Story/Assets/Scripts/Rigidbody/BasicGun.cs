using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicGun : MonoBehaviour
{
    //Gun Stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

   public int bulletsLeft, bulletsShot;

    //bools
   public bool shooting, readyToShoot, reloading;

    //References
    public TrailRenderer tracerEffect;
    public Camera camera;
    public Transform rayCastOrigin;
    public Transform rayCastDestination;
    public InputHandler inputHandler;
    public Ray ray;
    public RaycastHit raycastHit;

    //bug fixing
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        camera = Camera.main;
        inputHandler = GetComponent<InputHandler>();
        rayCastDestination = GameObject.FindGameObjectWithTag("Target").transform;
    }

    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        
        if (!inputHandler.playerInventory.rightWeapon.isMelee)
        {
            rayCastOrigin = GameObject.FindGameObjectWithTag("Gun").transform;
          
            if (allowButtonHold)
            {
                shooting = inputHandler.leftClickInput;
            }
            else
            {
                shooting = inputHandler.leftClickTapFlag;
            }

            if (inputHandler.reloadInput && bulletsLeft < magazineSize && !reloading)
            {
                Reload();
            }
            if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            {
                Reload();
            }
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) 
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit Position using a raycast
        ray.origin = rayCastOrigin.position;
        ray.direction = rayCastDestination.position - rayCastOrigin.position;

        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

        if(Physics.Raycast(ray, out raycastHit))
        {
            tracer.transform.position = raycastHit.point;

            if(raycastHit.transform.gameObject.tag=="Enemy")
            {
                EnemyStats enemyStats = raycastHit.transform.GetComponent<EnemyStats>();

                if(enemyStats != null)
                {
                    enemyStats.TakeDamage(3);
                    enemyStats.enemyManager.enemyLocomotionManager.currentTarget = GetComponent<PlayerStats>();
                }
            }
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
