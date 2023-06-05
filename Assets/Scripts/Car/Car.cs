using System;
using UnityEngine;

public abstract class Car : MonoBehaviour, IDieable
{
    [SerializeField] protected CarView View;
    [SerializeField] private CarMovement _movement;
    [SerializeField] private Rigidbody _rigidbody;
    
    public float Speed => _rigidbody.velocity.magnitude;
    public event Action Died;

    private float _vertical, _horizontal, _breaking;

    private void FixedUpdate()
    {
        GetMovementInputs(out _vertical, out _horizontal, out _breaking);
        _movement.ApplyBreaking(_breaking);
        _movement.Move(_vertical);
        _movement.Rotate(_horizontal);
    }

    public void Die()
    {
        _rigidbody.velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        OnDie();
        Died?.Invoke();
    }

    protected abstract void GetMovementInputs(out float vertical, out float horizontal, out float breaking);

    protected abstract void OnDie();
}