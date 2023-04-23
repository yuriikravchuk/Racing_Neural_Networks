using AI;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : Car
{
    [SerializeField] private List<Transform> _rayPositions;

    public NeuralNetwork Ai { get; private set; }
    public event Action<Enemy> Died;
    public event Action ScoreChanged;
    public int Score { get; private set; }
    public void Init(NeuralNetworkParameters AIParameeters = new NeuralNetworkParameters()) => Ai = new NeuralNetwork(AIParameeters);

    public void AddScore(int value)
    {
        if(value <= 0)
            throw new ArgumentOutOfRangeException();

        Score = value;
        ScoreChanged?.Invoke();
    }

    protected override void GetMovementInputs(out float vertical, out float horizontal, out float breaking)
    {
        ShowRay();
        vertical = 0;
        horizontal = 0;
        breaking = 0;
    }

    protected override void OnDie()
    {
        Died?.Invoke(this);
    }

    private void ShowRay()
    {
        foreach (var point in _rayPositions)
            Debug.DrawLine(point.transform.position, point.transform.forward * 100, Color.red);
    }
}