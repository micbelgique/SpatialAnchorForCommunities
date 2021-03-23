using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackToCamera : MonoBehaviour
{
    private Transform _camera;
    Vector3 targetAngle = Vector3.zero;

    private void Start()
    {
        if (!(Camera.main is null)) _camera = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(_camera);
        targetAngle = transform.localEulerAngles;
        targetAngle.x = 0;
        targetAngle.z = 0;
        targetAngle.y -= 180;
        transform.localEulerAngles = targetAngle;
    }

}