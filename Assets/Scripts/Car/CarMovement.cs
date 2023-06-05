using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _maxMotorForce;
    [SerializeField] private float _maxRotateAngle;


    public void Move(float vertical)
    {
        _colliderFL.motorTorque = _maxMotorForce * vertical;
        _colliderFR.motorTorque = _maxMotorForce * vertical;
    }

    public void Rotate(float horizontal)
    {
        _colliderFL.steerAngle = horizontal * _maxRotateAngle;
        _colliderFR.steerAngle = horizontal * _maxRotateAngle;
    }

    public void ApplyBreaking(float breakForce)
    {
        _colliderFL.brakeTorque = breakForce;
        _colliderFR.brakeTorque = breakForce;
        _colliderBL.brakeTorque = breakForce;
        _colliderBR.brakeTorque = breakForce;
    }

    public void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}