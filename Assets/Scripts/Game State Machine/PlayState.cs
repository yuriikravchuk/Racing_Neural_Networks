using UnityEngine;

public class PlayState : GameState
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private PlayUI _ui;
    [SerializeField] private Player _player;

    public override bool CanTransit(GameState state)
    {
        switch (state)
        {
            case MenuState:
                return true;

            default:
                return false;
        }
    }

    public override void Enter()
    {
        _enemySpawner.SpawnEnemies();
        _ui.ShowPlaySpeed();
    }

    public override void UpdateState()
    {
        _player.MovementVector.x = Input.GetAxis("Horizontal");
        _player.MovementVector.y = Input.GetAxis("Vertical");
        _ui.SetPlayerSpeed((int)_player.Speed);
    }

    public override void Exit()
    {
        _ui.HidePlaySpeed();
        _enemySpawner.ReturnAll();
    }
}