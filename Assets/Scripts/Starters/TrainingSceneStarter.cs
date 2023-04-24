using pool;
using UnityEngine;
using AI;

public class TrainingSceneStarter : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private AITrainer _aITrainer;
    [SerializeField] private float _timeScale = 1;

    private Pool<Enemy> enemyProvider;
    
    private void Awake()
    {
        enemyProvider = new Pool<Enemy>(_enemyPrefab);
        _aITrainer.Init(enemyProvider);
    }

    private void OnValidate()
    {
        if (_timeScale < 0.05f)
            _timeScale = 0.05f;
        Time.timeScale = _timeScale;
    }
}
