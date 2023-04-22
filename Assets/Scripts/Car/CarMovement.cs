using System;
using UnityEngine;
using UnityEngine.Events;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;

    [SerializeField] private float _breakForce;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _maxAngle;

    public event Action<Enemy> Died;

    public void Move(float vertical)
    {
        _colliderFL.motorTorque = _maxForce * vertical;
        _colliderFR.motorTorque = _maxForce * vertical;
    }

    public void Rotate(float horizontal)
    {
        _colliderFL.steerAngle = horizontal * _maxAngle;
        _colliderFR.steerAngle = horizontal * _maxAngle;
    }

    public void ApplyBreaking(float breakForce)
    {
        _colliderFL.brakeTorque = breakForce;
        _colliderFR.brakeTorque = breakForce;
        _colliderBL.brakeTorque = breakForce;
        _colliderBR.brakeTorque = breakForce;
    }


}
