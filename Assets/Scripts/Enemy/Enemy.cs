using UnityEngine;
public class Enemy : Car
{
    private EnemyAI _ai;

    public void Init(EnemyAI ai) => _ai = ai;

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        vertical = 0;// Input.GetAxis("Vertical");
        horizontal = 0;// Input.GetAxis("Horizontal");
        breaking = 0;//Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void GetBounds()
    {
        
    }
}