using UnityEngine;

public class CountingState : GameState
{
    [SerializeField] private PlayUI _ui;

    //private GameStateMachine _stateMachine;
    //private float _currentTime;

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
        //_ui.ShowCountingTime();
        //_currentTime = 3f;
        _ui.StartCountdown();
        //StartCoroutine(CountToZero());
    }

    public override void Exit()
    {
        //_ui.HideCountingTime();
        _ui.ShowGoText();
    }

    //private IEnumerator CountToZero()
    //{
    //    while (_currentTime > 0)
    //    {
    //        float deltaTime = Time.deltaTime;
    //        _currentTime = Mathf.Max(_currentTime - deltaTime, 0);
    //        _ui.SetCountingTime(_currentTime);
    //        yield return null;
    //    }

    //    _stateMachine.TrySwitchState<PlayState>();
    //    yield break;

   // }

    public override void UpdateState(){ }
}
