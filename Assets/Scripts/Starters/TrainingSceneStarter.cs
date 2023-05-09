using pool;
using UnityEngine;
using AI;
using System.Collections.Generic;

public class TrainingSceneStarter : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private AITrainer _aITrainer;
    [SerializeField] private UI _ui;
    [SerializeField] private float _timeScale = 1;

    private Pool<Enemy> _enemyProvider;
    private SaveBinder _saveBinder;
    
    private void Awake()
    {
        var saveProvider = new JsonSaveProvider<List<TrainingResults>>();
        WeightsBalancer weightsBalancer = new WeightsBalancer();
        _saveBinder = new SaveBinder(saveProvider, weightsBalancer);
        _ui.SaveButtonClicked += _saveBinder.Save;
        _ui.LoadButtonClicked += _saveBinder.Load;

        _enemyProvider = new Pool<Enemy>(_enemyPrefab);
        _aITrainer.Init(_enemyProvider, weightsBalancer);
    }

    private void OnValidate()
    {
        if (_timeScale < 0.05f)
            _timeScale = 0.05f;
        Time.timeScale = _timeScale;
    }
}
