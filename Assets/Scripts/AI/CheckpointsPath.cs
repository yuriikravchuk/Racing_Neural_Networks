using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsPath : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _path;

    public IReadOnlyList<Checkpoint> Path => _path;

    private void Start()
    {
        for(int i = 1; i < _path.Count; i++)
        {
            _path[i].Points = Vector3.Distance(_path[i].transform.position, _path[i - 1].transform.position);
        }
    }
}
