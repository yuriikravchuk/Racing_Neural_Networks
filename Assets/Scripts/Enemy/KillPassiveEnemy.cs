using UnityEngine;

public class KillPassiveEnemy : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private float _timeToAppendScore = 10;

    private float _lastCollectedTime;

    private void Start() => _enemy.ScoreChanged += OnScoreChanged;

    private void OnEnable() => _lastCollectedTime = Time.time;

    private void FixedUpdate()
    {
        if(Time.time > _lastCollectedTime + _timeToAppendScore)
            _enemy.Die();
    }

    public void OnScoreChanged() => _lastCollectedTime = Time.time;
}