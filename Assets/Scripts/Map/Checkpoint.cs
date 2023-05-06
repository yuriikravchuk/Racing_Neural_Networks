using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private float _maxPoints = 5;
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other?.GetComponent<Enemy>();
        if (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            enemy.AddPoints(_maxPoints - distance / 100);
        }

    }
}
