using UnityEngine;

public abstract class Car : MonoBehaviour, IDieable
{
    [SerializeField] private CarMovement _movement;
    [SerializeField] private Rigidbody _rigidbody;
    
    public float Speed => _rigidbody.velocity.magnitude;

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
        OnDie();
        gameObject.SetActive(false);
    }

    protected abstract void GetMovementInputs(out float vertical, out float horizontal, out float breaking);

    protected abstract void OnDie();
}