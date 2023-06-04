using UnityEngine;

public class Player : Car
{
    public Vector2 MovementVector;

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        vertical = MovementVector.y;
        horizontal = MovementVector.x;
        breaking = vertical == 0 ? 1 : 0;
    }

    protected override void OnDie() => MovementVector = Vector2.zero;
}