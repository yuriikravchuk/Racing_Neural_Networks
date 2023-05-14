using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float Points = 1f;
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other?.GetComponent<Enemy>();
        if (enemy != null)
        {
            //float distance = Vector3.Distance(transform.position, enemy.transform.position);
            //var points = MaxPoints - distance;
            enemy.AddPoints(Points);
        }
    }
}
