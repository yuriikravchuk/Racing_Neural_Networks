using System;
using UnityEngine;

public class Player : Car
{
    public event Action Died;

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        breaking = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
    protected override void OnDie()
    {
        Died?.Invoke();
    }
}