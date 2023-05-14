using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float Points = 1f;
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other?.GetComponent<Enemy>();
        if (enemy != null)
            enemy.AddPoints(Points);

    }
}
