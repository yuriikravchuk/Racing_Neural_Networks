using AI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Enemy : Car
{
    [SerializeField] private List<Transform> _rayPositions;

    public NeuralNetwork Ai { get; private set; }
    public int Score { get; private set; }
    public event Action<Enemy> Died;
    public event Action ScoreChanged;

    private NeuralNetworkParameters _networkParameters = new NeuralNetworkParameters(4, 4, 5, 5);
    private IReadOnlyList<Checkpoint> _path;
    private int _checkpointIndex;
    
    public void Init(IReadOnlyList<Checkpoint> path)
    {
        Ai = new NeuralNetwork(_networkParameters);
        _path = path;
    }

    private void OnEnable()
    {
        _checkpointIndex = 0;
        Score = 0;
    }

    public void AddPoints(int value)
    {
        if(value < 0)
            throw new ArgumentOutOfRangeException();

        Score += value;
        ScoreChanged?.Invoke();
    }

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        List<float> inputs = new();
        inputs.AddRange(GetDistancesToBorders());
        inputs.Add(GetDistanceToCheckpoint());
        float[] outputs = Ai.GetOutputs(inputs.ToArray());
        vertical = outputs[0] - outputs[1];
        horizontal = outputs[2] - outputs[3];
        breaking = 0;
        //Debug.Log("in: " + inputs[0].ToString() + " " + inputs[1].ToString() + " " + inputs[2].ToString() + "\n out: v:" + vertical.ToString() + " h: " + horizontal.ToString() + " b: " + breaking.ToString());
    }

    protected override void OnDie()
    {
        //Score -= GetDistanceToCheckpoint();
        Died?.Invoke(this);
    }

    private float[] GetDistancesToBorders()
    {
        var result = new float[_rayPositions.Count];
        for(int i = 0; i < _rayPositions.Count; i++)
        {
            Ray ray = new Ray(_rayPositions[i].transform.position, _rayPositions[i].transform.forward);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                float distance = hit.distance;
                result[i] = distance;
            }

            //Debug.DrawLine(_rayPositions[i].transform.position, _rayPositions[i].transform.forward * 100, Color.white, 0.01f ,true);
        }
        return result;
    }

    private float GetDistanceToCheckpoint() => Vector3.Distance(_path[_checkpointIndex].transform.position, transform.position);
}