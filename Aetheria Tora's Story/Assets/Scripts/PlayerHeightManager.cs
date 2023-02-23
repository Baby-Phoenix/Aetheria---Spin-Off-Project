using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeightManager : MonoBehaviour
{
    private CharacterController characterController;
    public Transform topPoint;
    public Transform bottomPoint;
    private float originalHeight;
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
    }

    public void SetCharacterControllerValues(bool isCrouch) 
    {
        float height = Vector3.Distance(topPoint.position, bottomPoint.position);
        characterController.height = height;


        if (isCrouch)
            characterController.center = Vector3.down * (originalHeight - characterController.height) / 2;

        else
            characterController.center = Vector3.up * (characterController.height - originalHeight) / 2;
    }
}
