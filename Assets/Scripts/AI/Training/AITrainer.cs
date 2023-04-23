using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AI
{
    public class AITrainer : MonoBehaviour
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private int _populationCount = 10;
        private IObjectProvider<Enemy> _enemiesProvider;
        private List<Enemy> _enemies;
        private List<TrainingResults> _results;
        private WeightsBalancer _weightsBalancer;
        private TrainingResults[] _bestResults;
        public void Init(IObjectProvider<Enemy> enemiesProvider, TrainingResults[] bestResults = null)
        {
            _enemiesProvider = enemiesProvider;
            _bestResults = bestResults ?? new TrainingResults[2];
        }

        public void StartTraining()
        {
            for (int i = 0; i < _populationCount; i++)
            {
                Enemy enemy = _enemiesProvider.Get();
                enemy.Died += OnEnemyDie;
                _enemies.Add(enemy);
            }


        }

        public void EndTraining()
        {
            foreach (var enemy in _enemies)
                enemy.Die();

            _bestResults = GetBestResults();

        }

        private TrainingResults[] GetBestResults()
        {
            _results.OrderBy(element => element.Score);
            return _results.Take(2).ToArray();
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
            _results.Add(new TrainingResults(enemy.Ai.CopyWeights(), enemy.Score));
        }
    }
}