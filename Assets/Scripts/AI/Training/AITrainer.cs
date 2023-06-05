using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace AI
{
    public class AITrainer : MonoBehaviour
    {
        [SerializeField] private int _populationCount = 10;
        [SerializeField] private Map _currentMap;

        public event Action<int> AliveCarsCountChanged;

        private WeightsBalancer _weightsBalancer;
        private IObjectProvider<Enemy> _enemiesProvider;
        private List<Enemy> _enemies;
        private List<TrainingResults> _results;
        private IReadOnlyList<int> _layersSize;
        private const int _maxBestCount = 2;

        public void Init(IObjectProvider<Enemy> enemiesProvider, WeightsBalancer weightsBalancer)
        {
            _weightsBalancer = weightsBalancer;
            _enemiesProvider = enemiesProvider;
            _enemies = new List<Enemy>();
            _results = new List<TrainingResults>();
            _layersSize = new List<int>() { 10, 4, 2 };
        }

        public void StartTraining()
        {
            _enemies.Clear();
            _results.Clear();
            SpawnBest();
            SpawnMainPopulation();
            AliveCarsCountChanged.Invoke(_enemies.Count);
        }

        private void SpawnBest()
        {
            if (_weightsBalancer.Results == null)
                return;

            foreach (var parent in _weightsBalancer.Results)
            {
                Enemy enemy = GetEnemy(parent.Neurons);
                enemy.SetParentColor();
            }

        }

        private void SpawnMainPopulation()
        {
            for (int i = 0; i < _populationCount; i++)
            {
                Neuron[][] neurons = _weightsBalancer.Results == null || _weightsBalancer.Results.Count < 2
                    ? _weightsBalancer.GetRandomNeurons(_layersSize)
                    : _weightsBalancer.UniformCross(1f);
                Enemy enemy = GetEnemy(neurons);
                enemy.SetDefaultColor();
            }
        }

        private Enemy GetEnemy(Neuron[][] neurons)
        {
            Enemy enemy = _enemiesProvider.Get();
            enemy.Init(_currentMap.Path, neurons);
            enemy.Died += () => OnEnemyDie(enemy);
            _enemies.Add(enemy);
            enemy.transform.SetPositionAndRotation(_currentMap.StartPosition, _currentMap.StartRotation);
            enemy.Ai.SetNeurons(neurons);
            return enemy;
        }

        public void EndTraining()
        {
            foreach (var enemy in _enemies)
                enemy.Die();

            _results = _results.OrderByDescending(element => element.Fitness).ToList();
            _weightsBalancer.SetResults(_results.Take(_maxBestCount));
            Debug.Log(_results[0].Fitness);
            StartTraining();
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
            float fitness = enemy.Score / _currentMap.MaxPoints;
            _results.Add(new TrainingResults(enemy.Ai.CopyNeurons(), fitness));
            enemy.Died -= () => OnEnemyDie(enemy);
            if (_enemies.Count == 0)
                EndTraining();

            AliveCarsCountChanged?.Invoke(_enemies.Count);
        }
    }
}