using UnityEngine;

public class PlaySceneStarter : MonoBehaviour
{
    [SerializeField] private PlayUI _ui;
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameStateMachine _gameStateMachine;


    private void Awake()
    {
        var save = new SaveBinder().Load();
        _enemySpawner.Init(save);

        _ui.PlayButtonClicked += () => _gameStateMachine.TrySwitchState<CountingState>();
        _ui.ÑountdownCompleted += () => _gameStateMachine.TrySwitchState<PlayState>();
        _player.Died += () => _gameStateMachine.TrySwitchState<MenuState>();

    }
}
