using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Checkpoint[] _path;
    public IReadOnlyList<Checkpoint> Path => _path;
    public Vector3 StartPosition => _startTransform.position;
    public Quaternion StartRotation => _startTransform.rotation;    
    public float MaxPoints { get; private set; }

    //private Checkpoint[] _path;

    private void Awake()
    {
        _path = GetComponentsInChildren<Checkpoint>();

        for (int i = 1; i < _path.Length; i++)
        {
            float points = Vector3.Distance(_path[i].transform.position, _path[i - 1].transform.position);
            _path[i].Init(i, points);
            MaxPoints += points;
        }
    }
}