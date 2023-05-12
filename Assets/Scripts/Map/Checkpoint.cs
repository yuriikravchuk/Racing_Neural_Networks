using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private float _maxPoints = 5f;
    [SerializeField] private float _minPoints = 0.05f;
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other?.GetComponent<Enemy>();
        if (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            var points = _maxPoints - distance;
            enemy.AddPoints(Mathf.Max(points, _minPoints));
        }
    }
}
