using AI;
using pool;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private MapsHandler _mapsHandler;

    private IObjectProvider<Enemy> enemiesProvider;
    private IEnumerable<Neuron[][]> _brains;
    private List<Enemy> _enemies;

    public void Init(IReadOnlyList<Neuron[][]> brains)
    {
        enemiesProvider = new Pool<Enemy>(_enemyPrefab);
        _brains = brains;
        _enemies = new List<Enemy>();
    }

    public void SpawnEnemies()
    {
        foreach (var brain in _brains)
            _enemies.Add(GetEnemy(brain, _mapsHandler.CurrentMap));
    }

    private Enemy GetEnemy(Neuron[][] brain, Map map)
    {
        Enemy enemy = enemiesProvider.Get();
        enemy.Init(map.Path, brain);
        enemy.transform.SetPositionAndRotation(map.StartPosition, map.StartRotation);
        return enemy;
    }

    public void ReturnAll()
    {
        foreach (var enemy in _enemies)
            enemy.Die();
    }
}
