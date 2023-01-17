using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDash : MonoBehaviour
{
    private CharacterControllerPlayerMovement playerMovement;
    private InputManager inputManager;
    
    public float dashSpeed;
    public float dashTime;
    public float dashCoolDown;
    private float coolDownTimer;
    private bool isDashing = false;
    private bool canDash = true;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<CharacterControllerPlayerMovement>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canDash)
        {
            coolDownTimer = dashCoolDown;
            if (inputManager.Slide)
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            if (!isDashing)
            {
                coolDownTimer -= Time.deltaTime;
                canDash = coolDownTimer <= 0;
            }
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            playerMovement.characterController.Move(playerMovement.targetMotion.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}