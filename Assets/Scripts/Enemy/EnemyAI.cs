using UnityEngine;
using AI;
public class EnemyAI : NeuralNetwork
{
    public int Points;

    public EnemyAI() : base(new NeuralNetworkParameters())
    {

    }

    public Vector2 GetMoveVector(Vector3 position, Vector3 targetPosition, float speed)
    {
        return Vector2.zero;
    }
}
