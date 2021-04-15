using UnityEngine;

[ExecuteInEditMode]
public class FaceTowardsCamera : MonoBehaviour
{
    private Transform _camera;
    Vector3 targetAngle = Vector3.zero;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        gameObject.transform.LookAt(_camera);
        targetAngle = gameObject.transform.localEulerAngles;
        targetAngle.x = 0;
        targetAngle.z = 0;
        targetAngle.y -= 360;
        gameObject.transform.localEulerAngles = targetAngle;
    }

}