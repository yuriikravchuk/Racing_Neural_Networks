using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private CheckpointsPath _checkpointsPath;
    [SerializeField] private Transform _startTransform;
    public IReadOnlyList<Checkpoint> Path => _checkpointsPath.Path;
    public Vector3 StartPosition => _startTransform.position;
    public Quaternion StartRotation => _startTransform.rotation;    
    public float MaxPoints => _checkpointsPath.MaxPoints;
}