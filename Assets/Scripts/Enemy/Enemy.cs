using AI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Enemy : Car
{
    [SerializeField] private List<Transform> _rayPositions;

    public NeuralNetwork Ai { get; private set; }
    public float Score { get; private set; }
    public event Action<Enemy> Died;
    public event Action ScoreChanged;

    private IReadOnlyList<Checkpoint> _path;
    private int _checkpointIndex;
    
    public void Init(IReadOnlyList<Checkpoint> path, IReadOnlyList<int> LayersSize)
    {
        Ai = new NeuralNetwork(LayersSize);
        _path = path;
    }

    private void OnEnable()
    {
        _checkpointIndex = 0;
        Score = 0;
    }

    public void AddPoints(float value)
    {
        if(value < 0)
            throw new ArgumentOutOfRangeException();

        Score += value;
        ScoreChanged?.Invoke();
        _checkpointIndex++;
        if(_checkpointIndex >= _path.Count)
        {
            Score += 20;
            Die();
        }
    }

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        List<float> inputs = new();
        inputs.AddRange(GetDistancesToBorders());
        inputs.Add(GetDistanceToCheckpoint());
        inputs.Add(Speed);
        inputs.Add(GetCheckpointRotationDifference());
        IReadOnlyList<float> outputs = Ai.GetOutputs(inputs);
        vertical = outputs[0];
        horizontal = outputs[1];
        breaking = 0;
        //Debug.Log("in: " + inputs[0].ToString() + " " + inputs[1].ToString() + " " + inputs[2].ToString() + inputs[3].ToString() + " " + inputs[4].ToString() + " " + inputs[5].ToString() + " " + inputs[6].ToString() + " " + inputs[7].ToString() + " " + "\n out: v:" + vertical.ToString() + " h: " + horizontal.ToString() + " b: " + breaking.ToString());
        //Debug.Log("v:" + vertical.ToString() + " h: " + horizontal.ToString());
    }

    protected override void OnDie()
    {
        Score -= GetDistanceToCheckpoint() / 100;
        Died?.Invoke(this);
    }

    private float[] GetDistancesToBorders()
    {
        var result = new float[_rayPositions.Count];
        for(int i = 0; i < _rayPositions.Count; i++)
        {
            Ray ray = new(_rayPositions[i].transform.position, _rayPositions[i].transform.forward);

            if(Physics.Raycast(ray, out RaycastHit hit))
                result[i] = hit.distance;

            //Debug.DrawLine(_rayPositions[i].transform.position, _rayPositions[i].transform.forward * 100, Color.white, 0.01f ,true);
        }
        return result;
    }

    private float GetCheckpointRotationDifference()
    {
        var VectorToRotate = _path[_checkpointIndex].transform.position - transform.position;
        Debug.DrawRay(transform.position, VectorToRotate);
        //Debug.DrawLine(transform.position, _path[_checkpointIndex].transform.position);
        return Vector3.Angle(transform.position, VectorToRotate);
    }

    private float GetDistanceToCheckpoint()
    {
        var checkpointPosition = _path[_checkpointIndex].transform.position;
        checkpointPosition.y = 0;
        return Vector3.Distance(checkpointPosition, transform.position);
    }
}