using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
    [SerializeField] private List<Collider> _colliders;

    private void OnTriggerEnter(Collider other)
    {
        var instance = other?.GetComponent<IDieable>();
        if (instance != null)
        {
            instance.Die();
        }
    }
}
