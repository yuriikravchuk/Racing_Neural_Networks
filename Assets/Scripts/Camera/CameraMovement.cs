using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed;

    private void FixedUpdate()
    {
        transform.position = _target.position;
        var targetRotation = Quaternion.Euler(0, y: _target.rotation.eulerAngles.y, 0);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed);
        transform.rotation = targetRotation;
    }

}
