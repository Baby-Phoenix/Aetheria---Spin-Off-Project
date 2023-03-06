using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera camera;
    Ray ray;
    RaycastHit hitinfo;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        ray.origin = camera.transform.position;
        ray.direction = camera.transform.forward;

        Physics.Raycast(ray, out hitinfo);
        transform.position = hitinfo.point;

    }
}
