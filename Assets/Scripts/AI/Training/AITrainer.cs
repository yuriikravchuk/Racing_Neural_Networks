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
        private List<Enemy> _enemies;
        private List<TrainingResults> _results;
        private List<Neuron[][]> _best;


        private float _iteratiomCount;

        public void Init(IObjectProvider<Enemy> enemiesProvider, List<Neuron[][]> bestWeights = null)
        {
            _weightsBalancer = new WeightsBalancer();
            _enemiesProvider = enemiesProvider;
            _enemies = new List<Enemy>();
            _results = new List<TrainingResults>();
            _best = bestWeights;
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
                enemy.Ai.SetNeurons(_weightsBalancer.GetRandomNeurons(enemy.Ai.Parameters));
                //_weightsBalancer.SetRandomNeurons(enemy.Ai);
            }
        }

        private void SpawnMainPopulation()
        {
            for (int i = 0; i < _populationCount; i++)
            {
                Enemy enemy = GetEnemy();
                var neurons = _best == null || _best.Count < 2
                    ? _weightsBalancer.GetRandomNeurons(enemy.Ai.Parameters) 
                    : _weightsBalancer.UniformCross(_best);

                enemy.Ai.SetNeurons(neurons);
            }
        }

        private Enemy GetEnemy()
        {
            Enemy enemy = _enemiesProvider.Get();
            enemy.Init(_checkpointsPath.Path);
            enemy.Died += OnEnemyDie;
            _enemies.Add(enemy);
            enemy.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);

            return enemy;
        }

        public void EndTraining()
        {
            foreach (var enemy in _enemies)
                enemy.Die();

            var sortedByScore = _results.OrderByDescending(element => element.Score).ToList();
            _best = sortedByScore.Take(2).Select(element => element.Neurons).ToList();

            //Debug.Log(sortedByScore[0].Score.ToString() + " "+ sortedByScore[1].Score.ToString());
            //Debug.Log("Best: " + _results[0].Score.ToString() + "Count: " + _iteratiomCount.ToString());
            Debug.Log("Count: " + _iteratiomCount.ToString());
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