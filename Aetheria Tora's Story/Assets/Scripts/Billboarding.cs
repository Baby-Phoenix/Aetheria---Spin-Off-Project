using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    public Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }
}
