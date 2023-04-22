using UnityEngine;

public abstract class Player: Car
{
    [SerializeField] private Car _car;

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        breaking = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}