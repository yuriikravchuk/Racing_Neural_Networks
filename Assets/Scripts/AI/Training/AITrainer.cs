using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace AI
{
    public class AITrainer : MonoBehaviour
    {
        [SerializeField] private MapsHandler _mapsHandler;
        //[SerializeField] private Transform startTransform;
        [SerializeField] private int _populationCount = 10;
        [SerializeField] private int _randomPopulationCount = 10;
        //[SerializeField] private CheckpointsPath _checkpointsPath;

        private List<Enemy> _enemies;
        private WeightsBalancer _weightsBalancer;
        private IObjectProvider<Enemy> _enemiesProvider;
        private IReadOnlyList<int> _layersSize;
 
        private List<TrainingResults> _results;
        private const int _minBestCount = 2, _maxBestCount = 4;
        private float _iteratiomCount;

        public void Init(IObjectProvider<Enemy> enemiesProvider, WeightsBalancer weightsBalancer, List<Neuron[][]> best = null)
        {
            _weightsBalancer = weightsBalancer;
            _enemiesProvider = enemiesProvider;
            _enemies = new List<Enemy>();
            _results = new List<TrainingResults>();
            _layersSize = new List<int>() { 8, 10, 10, 10, 10, 2 };
        }

        public void StartTraining()
        {
            _enemies.Clear();
            _results.Clear();

            Map map = _mapsHandler.GetNextMap();

            SpawnBest(map);
            SpawnMainPopulation(map);
            SpawnRandomPopulation(map);
            _iteratiomCount++;
        }

        private void SpawnBest(Map map)
        {
            if (_weightsBalancer.Parents == null)
                return;

            foreach (var neurons in _weightsBalancer.Parents)
                SpawnEnemy(neurons.Neurons, map);

        }

        private void SpawnRandomPopulation(Map map)
        {
            for (int i = 0; i < _randomPopulationCount; i++)
            {
                Neuron[][] neurons = _weightsBalancer.GetRandomNeurons(_layersSize);
                SpawnEnemy(neurons, map);
            }
        }

        private void SpawnMainPopulation(Map map)
        {
            for (int i = 0; i < _populationCount; i++)
            {
                Neuron[][] neurons = _weightsBalancer.Parents == null || _weightsBalancer.Parents.Count < 2
                    ? _weightsBalancer.GetRandomNeurons(_layersSize) 
                    : _weightsBalancer.UniformCross();
                SpawnEnemy(neurons, map);
            }
        }

        private void SpawnEnemy(Neuron[][] neurons, Map map)
        {
            Enemy enemy = _enemiesProvider.Get();
            enemy.Init(map.Path, _layersSize);
            enemy.Died += OnEnemyDie;
            _enemies.Add(enemy);
            enemy.transform.SetPositionAndRotation(map.StartPosition, map.StartRotation);
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
            _weightsBalancer.AddParents(_results.Take(_maxBestCount).ToList());
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