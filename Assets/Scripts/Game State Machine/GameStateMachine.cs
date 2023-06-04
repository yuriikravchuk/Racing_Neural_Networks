using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour 
{
    [SerializeField] private List<GameState> _states;
    private GameState _currentState;


    private void Awake()
    {
        _currentState = FindState<MenuState>();
        _currentState.Enter();
    }

    private void Update() => _currentState.UpdateState();

    public void TrySwitchState<T>(){
        GameState nextState = FindState<T>();
        if (_currentState.CanTransit(nextState))
            SwitchState(nextState);
    }

    private void SwitchState(GameState state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    private GameState FindState<T>() 
        => _states.Find(x => x is T) ?? throw new InvalidOperationException();
}