using UnityEngine;

public class Borders : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var instance = other?.GetComponent<IDieable>();
        if (instance != null)
            instance.Die();
    }
}
