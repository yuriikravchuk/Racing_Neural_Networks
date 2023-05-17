using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int _index;
    private float _points;

    public void Init(int index, float points)
    {
        _index = index;
        _points = points;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other?.GetComponent<Enemy>();
        if (enemy != null)
            enemy.CheckpointCollected(_index, _points);
    }
}
