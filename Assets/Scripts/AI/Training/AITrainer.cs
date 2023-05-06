using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace AI
{
    public class AITrainer : MonoBehaviour
    {
        [SerializeField] private Transform startTransform;
        [SerializeField] private int _populationCount = 10;
        [SerializeField] private int _randomPopulationCount = 10;
        [SerializeField] private CheckpointsPath _checkpointsPath;

        private List<Enemy> _enemies;
        private WeightsBalancer _weightsBalancer;
        private IObjectProvider<Enemy> _enemiesProvider;
        private IReadOnlyList<int> _layersSize;
 
        private List<TrainingResults> _results;
        private List<Neuron[][]> _bestNeurons;
        private const int _minBestCount = 2, _maxBestCount = 4;
        private float _iteratiomCount;

        public void Init(IObjectProvider<Enemy> enemiesProvider, List<Neuron[][]> best = null)
        {
            _weightsBalancer = new WeightsBalancer();
            _enemiesProvider = enemiesProvider;
            _enemies = new List<Enemy>();
            _results = new List<TrainingResults>();
            _bestNeurons = best;
            _layersSize = new List<int>() { 8, 4, 4, 4, 4, 2 };
        }

        public void StartTraining()
        {
            _enemies.Clear();
            _results.Clear();
            SpawnBest();
            SpawnMainPopulation();
            SpawnRandomPopulation();
            _iteratiomCount++;
        }

        private void SpawnBest()
        {
            if (_bestNeurons == null)
                return;

            foreach (var neurons in _bestNeurons)
                SpawnEnemy(neurons);

        }

        private void SpawnRandomPopulation()
        {
            for (int i = 0; i < _randomPopulationCount; i++)
            {
                Neuron[][] neurons = _weightsBalancer.GetRandomNeurons(_layersSize);
                SpawnEnemy(neurons);
            }
        }

        private void SpawnMainPopulation()
        {
            for (int i = 0; i < _populationCount; i++)
            {
                var neurons = _bestNeurons == null || _bestNeurons.Count < _maxBestCount
                    ? _weightsBalancer.GetRandomNeurons(_layersSize) 
                    : _weightsBalancer.UniformCross(_bestNeurons);
                SpawnEnemy(neurons);
            }
        }

        private void SpawnEnemy(Neuron[][] neurons)
        {
            Enemy enemy = _enemiesProvider.Get();
            enemy.Init(_checkpointsPath.Path, _layersSize);
            enemy.Died += OnEnemyDie;
            _enemies.Add(enemy);
            enemy.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
            enemy.Ai.SetNeurons(neurons);
        }

        public void EndTraining()
        {
            foreach (var enemy in _enemies)
                enemy.Die();

            UpdateBestResults();
            //Debug.Log("Count: " + _iteratiomCount.ToString());
            StartTraining();
        }

        private void UpdateBestResults()
        {
            _results = _results.OrderByDescending(element => element.Score).ToList();
            _bestNeurons = _results.Take(_maxBestCount).Select(element => element.Neurons).ToList();

            Debug.Log(_results[0].Score.ToString() + " " + _results[1].Score.ToString());
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
            _results.Add(new TrainingResults(enemy.Ai.CopyNeurons(), enemy.Score));
            enemy.Died -= OnEnemyDie;
            if (_enemies.Count == 0)
                EndTraining();
        }
    }
}