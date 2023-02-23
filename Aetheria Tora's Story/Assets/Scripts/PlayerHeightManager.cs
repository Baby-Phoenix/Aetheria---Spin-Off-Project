using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeightManager : MonoBehaviour
{
    private CharacterController characterController;
    public Transform topPoint;
    public Transform bottomPoint;
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void SetCharacterControllerValues(bool isCrouch) 
    {
        float height = Vector3.Distance(topPoint.position, bottomPoint.position);
        characterController.height = height;

        if (isCrouch)
            characterController.center = new Vector3(0, -0.39f, 0);

        else
            characterController.center = Vector3.zero;
    }
}
