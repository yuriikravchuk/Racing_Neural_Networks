using UnityEngine;

public class MenuState : GameState
{
    [SerializeField] private PlayUI _ui;
    [SerializeField] private Transform _camera;
    [SerializeField] private Player _player;
    [SerializeField] private MapsHandler _mapsHandler;  

    public override bool CanTransit(GameState state)
    {
        switch (state)
        {
            case CountingState:
                return true;
            default:
                return false;
        }
    }

    public override void Enter()
    {
        _mapsHandler.SwitchMap();
        Map map = _mapsHandler.CurrentMap;
        _camera.SetPositionAndRotation(map.StartPosition, map.StartRotation);
        _player.transform.SetPositionAndRotation(map.StartPosition, map.StartRotation);
        _ui.ShowPlayButton();
    }


    public override void Exit() => _ui.HidePlayButton();

    public override void UpdateState(){ }
}
