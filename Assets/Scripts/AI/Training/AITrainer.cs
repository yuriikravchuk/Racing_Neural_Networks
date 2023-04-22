using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using pool;

public class AITrainer : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private int _populationCount;
    private IObjectProvider<Enemy> _enemiesProvider;
    private List<Enemy> _enemies;

    private List<NeuralNetwork> _bestNetworks;
    public void Init(IObjectProvider<Enemy> enemiesProvider, List<NeuralNetwork> bestAI = null)
    {
        _enemiesProvider = enemiesProvider;
        _bestNetworks = bestAI == null ? new List<NeuralNetwork>() : bestAI;
    }


    public void StartTraining()
    {
        for(int i = 0; i < _populationCount; i++)
        {
            Enemy enemy = _enemiesProvider.Get();

            _enemies.Add(enemy);
        }
    }

    private void GetBestAI()
    {
        int[] bestScores = new int[3];

        foreach (var network in _enemies)
        {

        }
    }

    private void OnEnemyDie(IDieable car)
    {

    }
}
