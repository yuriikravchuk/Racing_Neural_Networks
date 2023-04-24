using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        private List<float[][,]> _bestWeights;


        private float _iteratiomCount;

        public void Init(IObjectProvider<Enemy> enemiesProvider, List<float[][,]> bestWeights = null)
        {
            _weightsBalancer = new WeightsBalancer();
            _enemiesProvider = enemiesProvider;
            _enemies = new List<Enemy>();
            _results = new List<TrainingResults>();
            _bestWeights = bestWeights;
            //Time.timeScale = 10f;
        }

        public void StartTraining()
        {
            _enemies.Clear();
            _results.Clear();
            //_results = new List<TrainingResults>();
            SpawnMainPopulation();
            SpawnRandomPopulation();
            _iteratiomCount++;
        }

        private void SpawnRandomPopulation()
        {
            for (int i = 0; i < _randomPopulationCount; i++)
            {
                Enemy enemy = GetEnemy();
                _weightsBalancer.SetRandomWeights(enemy.Ai);
            }
        }

        private void SpawnMainPopulation()
        {
            for (int i = 0; i < _populationCount; i++)
            {
                Enemy enemy = GetEnemy();

                if (_bestWeights == null)
                    _weightsBalancer.SetRandomWeights(enemy.Ai);
                else
                    _weightsBalancer.UniformCross(_bestWeights, enemy.Ai);
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
            {
                enemy.Die();
            }
            var sortedByScore = _results.OrderByDescending(element => element.Score).ToList();
            _bestWeights = sortedByScore.Take(2).Select(element => element.Weights).ToList();

            Debug.Log(sortedByScore[0].Score.ToString() + " "+ sortedByScore[1].Score.ToString());
            //Debug.Log("Best: " + _results[0].Score.ToString() + "Count: " + _iteratiomCount.ToString());
            
            StartTraining();
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
            _results.Add(new TrainingResults(enemy.Ai.CopyWeights(), enemy.Score));
            enemy.Died -= OnEnemyDie;
            if (_enemies.Count == 0)
                EndTraining();
        }
    }
}