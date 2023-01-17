using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamFPS : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    //this needs to be universal whereEver it goes, make another c# file that handles this
    private InputManager inputManager; 


    // Start is called before the first frame update
    void Awake()
    {
        inputManager = gameObject.GetComponentInParent<InputManager>();
    }
  
    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }



    private void HandleInput()
    {
        float mouseX = inputManager.Aim.x * Time.deltaTime * sensX;
        float mouseY = inputManager.Aim.y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate the cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);


        ///***MAKE SOMETHING IN PAUSE MENU THAT CAN CONTROL THE SENSITIVITY (senX, senY)*****
    }
}
