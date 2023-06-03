using pool;
using UnityEngine;
using AI;
using System.Collections.Generic;

public class TrainingSceneStarter : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private AITrainer _aITrainer;
    [SerializeField] private TrainingUI _ui;
    [SerializeField] private ScoreBoard _board;
    [SerializeField] private CarsAliveText _carsAliveText;

    private Pool<Enemy> _enemyProvider;
    private SaveBinder _saveBinder;
    
    private void Awake()
    {
        WeightsBalancer weightsBalancer = new WeightsBalancer();
        weightsBalancer.ScoresChanged += _board.UpdateScores;
        _saveBinder = new SaveBinder();
        var save = _saveBinder.Load();
        weightsBalancer.SetResults(save);
        _ui.SaveButtonClicked += () => _saveBinder.Save(weightsBalancer.Results);
        _enemyProvider = new Pool<Enemy>(_enemyPrefab);
        _aITrainer.Init(_enemyProvider, weightsBalancer);
        _aITrainer.AliveCarsCountChanged += _carsAliveText.OnCarsAliveCountChanged;
    }
}
