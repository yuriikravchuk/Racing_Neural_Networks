using UnityEngine;

public class CountingState : GameState
{
    [SerializeField] private PlayUI _ui;

    public override bool CanTransit(GameState state)
    {
        switch(state)
        {
            case PlayState:
                return true;
            default: 
                return false;
        }
    }

    public override void Enter()
    {
        _ui.StartCountdown();
    }

    public override void Exit()
    {
        _ui.ShowGoText();
    }

    public override void UpdateState(){ }
}
