using UnityEngine;

public class KillPassiveEnemy : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private float _timeToAppendScore;

    private float lastCollectedTime;

    private void Start() => _enemy.ScoreChanged += OnScoreChanged;

    private void OnEnable() => lastCollectedTime = Time.time;

    private void FixedUpdate()
    {
        if(Time.time > lastCollectedTime + _timeToAppendScore)
            _enemy.Die();
    }

    public void OnScoreChanged() => lastCollectedTime = Time.time;
}