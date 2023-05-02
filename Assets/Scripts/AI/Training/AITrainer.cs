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

        private WeightsBalancer _weightsBalancer;
        private IObjectProvider<Enemy> _enemiesProvider;
        private IReadOnlyList<int> _layersSize;
        private List<Enemy> _enemies;
        private List<TrainingResults> _results;
        private List<Neuron[][]> _best;
        private float _iteratiomCount;

        public void Init(IObjectProvider<Enemy> enemiesProvider, List<Neuron[][]> best = null)
        {
            _weightsBalancer = new WeightsBalancer();
            _enemiesProvider = enemiesProvider;
            _enemies = new List<Enemy>();
            _results = new List<TrainingResults>();
            _best = best;
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
            if (_best == null)
                return;

            foreach (var weights in _best)
            {
                Enemy enemy = GetEnemy();
                enemy.Ai.SetNeurons(weights);
            }
        }

        private void SpawnRandomPopulation()
        {
            for (int i = 0; i < _randomPopulationCount; i++)
            {
                Enemy enemy = GetEnemy();
                Neuron[][] neurons = _weightsBalancer.GetRandomNeurons(_layersSize);
                enemy.Ai.SetNeurons(neurons);
            }
        }

        private void SpawnMainPopulation()
        {
            for (int i = 0; i < _populationCount; i++)
            {
                Enemy enemy = GetEnemy();
                var neurons = _best == null || _best.Count < 2
                    ? _weightsBalancer.GetRandomNeurons(_layersSize) 
                    : _weightsBalancer.UniformCross(_best);

                enemy.Ai.SetNeurons(neurons);
            }
        }

        private Enemy GetEnemy()
        {
            Enemy enemy = _enemiesProvider.Get();
            enemy.Init(_checkpointsPath.Path, _layersSize);
            enemy.Died += OnEnemyDie;
            _enemies.Add(enemy);
            enemy.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);

            return enemy;
        }

        public void EndTraining()
        {
            foreach (var enemy in _enemies)
                enemy.Die();

            _results = _results.OrderByDescending(element => element.Score).Take(6).ToList();
            _best = _results.Take(2).Select(element => element.Neurons).ToList();

            Debug.Log(_results[0].Score.ToString() + " "+ _results[1].Score.ToString());
            //Debug.Log("Count: " + _iteratiomCount.ToString());
            StartTraining();
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