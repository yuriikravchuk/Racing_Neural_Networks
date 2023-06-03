using AI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Car
{
    [SerializeField] private List<Transform> _rayPositions;

    public NeuralNetwork Ai { get; private set; }
    public float Score { get; private set; }
    public float SpawnTime;
    public event Action ScoreChanged;

    private IReadOnlyList<Checkpoint> _path;
    private int _checkpointIndex;

    public void Init(IReadOnlyList<Checkpoint> path, Neuron[][] neurons)
    {
        Ai = new NeuralNetwork(neurons);
        _path = path;
    }

    private void OnEnable()
    {
        _checkpointIndex = 0;
        Score = 0;
        SpawnTime = Time.time;
    }

    public void CheckpointCollected(int index, float value)
    {
        if (index != _checkpointIndex)
            return;

        if (value < 0)
            throw new ArgumentOutOfRangeException();

        Score += value;
        ScoreChanged?.Invoke();
        _checkpointIndex++;
        if (_checkpointIndex >= _path.Count)
            Die();
    }

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        List<float> inputs = new();
        inputs.AddRange(GetDistancesToBorders());
        inputs.Add(GetDistanceToCheckpoint(_checkpointIndex));
        inputs.Add(Speed);
        inputs.Add(GetCheckpointRotationDifference());
        IReadOnlyList<float> outputs = Ai.GetOutputs(inputs);
        vertical = outputs[0];
        horizontal = outputs[1];
        breaking = 0;
    }

    protected override void OnDie()
    {
        if (_checkpointIndex > 0 && _checkpointIndex < _path.Count - 1)
            Score += GetDistanceToCheckpoint(_checkpointIndex - 1);
        
        gameObject.SetActive(false);
    }

    private float[] GetDistancesToBorders()
    {
        var result = new float[_rayPositions.Count];
        for (int i = 0; i < _rayPositions.Count; i++)
        {
            Ray ray = new(_rayPositions[i].transform.position, _rayPositions[i].transform.forward * 100);

            if (Physics.Raycast(ray, out RaycastHit hit))
                result[i] = hit.distance;
            Debug.DrawRay(_rayPositions[i].transform.position, _rayPositions[i].transform.forward * 40);
        }
        return result;
    }

    private float GetCheckpointRotationDifference()
    {
        var VectorToRotate = GetNormalVector(_checkpointIndex);
        Debug.DrawRay(transform.position, VectorToRotate, Color.yellow);
        return Vector3.Angle(transform.forward, VectorToRotate);
    }

    private float GetDistanceToCheckpoint(int index)
    {
        var checkpointPosition = _path[index].transform.position;
        checkpointPosition.y = 0;
        Vector3 VectorToRotate = GetNormalVector(index);
        return VectorToRotate.magnitude;
    }

    private Vector3 GetNormalVector(int index)
    {
        Transform checkpoint = _path[index].transform;
        Vector3 checkpointPosition = checkpoint.transform.position;
        checkpointPosition.y = 0;
        var vectorToCenter = checkpointPosition - transform.position;
        float angle = Vector3.SignedAngle(vectorToCenter, checkpoint.forward, Vector3.up);
        float a = vectorToCenter.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 normalPoint = checkpointPosition + checkpoint.right * a;
        Vector3 normalVector = normalPoint - transform.position;
        return normalVector;
    }

    public void SetDefaultColor() => View.SetColor(Color.green);

    public void SetParentColor() => View.SetColor(Color.blue);
}