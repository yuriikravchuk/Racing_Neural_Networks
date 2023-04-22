using UnityEngine;
using System;

public abstract class Car : MonoBehaviour, IDieable
{
    [SerializeField] private CarMovement _movement;

    public event Action<Car> Died;

    private void FixedUpdate()
    {
        GetMovementInputs(out float vertical, out float horizontal, out float breaking);
        _movement.ApplyBreaking(breaking);
        _movement.Move(vertical);
        _movement.Rotate(horizontal);
    }

    public void Die()
    {
        Died?.Invoke(this);
    }

    protected abstract void GetMovementInputs(out float vertical, out float horizontal, out float breaking);
}