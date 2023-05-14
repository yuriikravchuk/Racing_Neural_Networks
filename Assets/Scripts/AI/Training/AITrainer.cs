using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AI
{
    public class AITrainer : MonoBehaviour
    {
        [SerializeField] private MapsHandler _mapsHandler;
        [SerializeField] private int _populationCount = 10;
        [SerializeField] private int _randomPopulationCount = 10;

        private WeightsBalancer _weightsBalancer;
        private IObjectProvider<Enemy> _enemiesProvider;
        private Map _currentMap;
        private List<Enemy> _enemies;
        private List<TrainingResults> _results;
        private IReadOnlyList<int> _layersSize;
        private const int _maxBestCount = 4;

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

            _currentMap = _mapsHandler.GetNextMap();

            SpawnBest();
            SpawnMainPopulation();
            SpawnRandomPopulation();
        }

        private void SpawnBest()
        {
            if (_weightsBalancer.Parents == null)
                return;

            foreach (var neurons in _weightsBalancer.Parents)
                SpawnEnemy(neurons.Neurons);
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
                Neuron[][] neurons = _weightsBalancer.Parents == null || _weightsBalancer.Parents.Count < 2
                    ? _weightsBalancer.GetRandomNeurons(_layersSize) 
                    : _weightsBalancer.UniformCross();
                SpawnEnemy(neurons);
            }
        }

        private void SpawnEnemy(Neuron[][] neurons)
        {
            Enemy enemy = _enemiesProvider.Get();
            enemy.Init(_currentMap.Path, _layersSize);
            enemy.Died += OnEnemyDie;
            _enemies.Add(enemy);
            enemy.transform.SetPositionAndRotation(_currentMap.StartPosition, _currentMap.StartRotation);
            enemy.Ai.SetNeurons(neurons);
        }

        public void EndTraining()
        {
            foreach (var enemy in _enemies)
                enemy.Die();

            UpdateBestResults();
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
            float fitness = enemy.Score / _currentMap.MaxPoints;
            _results.Add(new TrainingResults(enemy.Ai.CopyNeurons(), fitness));
            enemy.Died -= OnEnemyDie;
            if (_enemies.Count == 0)
                EndTraining();
        }
    }
}