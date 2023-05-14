using System.Collections.Generic;
using UnityEngine;

public class CheckpointsPath : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _path;

    public IReadOnlyList<Checkpoint> Path => _path;
    public float MaxPoints { get; private set; }

    private void Start()
    {
        MaxPoints += _path[0].Points;
        for (int i = 1; i < _path.Count; i++)
        {
            float points = Vector3.Distance(_path[i].transform.position, _path[i - 1].transform.position);
            _path[i].Points = points;
            MaxPoints += points;
        }
    }
}
