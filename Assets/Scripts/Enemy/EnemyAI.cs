using UnityEngine;
using AI;
public class EnemyAI
{
    private NeuralNetwork _network;

    public EnemyAI(NeuralNetwork network)
    {
        _network = network;
    }

    public Vector2 GetMoveVector(Vector3 position, Vector3 targetPosition, float speed)
    {
        return Vector2.zero;
    }
}
