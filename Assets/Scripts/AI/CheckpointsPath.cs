using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsPath : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _path;

    public IReadOnlyList<Checkpoint> Path => _path;
}
