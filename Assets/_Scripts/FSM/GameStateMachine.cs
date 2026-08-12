using System;
using System.Collections.Generic;
using UnityEngine;
using VitalRouter;

//Event base state machine
[Routes]
public partial class GameStateMachine
{
    
    private readonly Dictionary<GameStateType, GameState> _states;
    private GameState _currentState;

    public GameState CurrentState => _currentState;
    public GameStateMachine(GameplayManager manager)
    {
        HomeState home = new HomeState(manager);
        IngameState inGame = new IngameState(manager);
        WinState win = new WinState();
        LoseState lose = new LoseState();
        _states = new()
        {
            [GameStateType.Home] = home,
            [GameStateType.InGame] = inGame,
            [GameStateType.Win] = win,
            [GameStateType.Lose] = lose
        };
        manager.FSM = this;
        ChangeState(GameStateType.Home);
    }
    
    private void ChangeState(GameStateType type)
    {
        if (_currentState == _states[type])
            return;
        _currentState?.OnExit();
        _currentState = _states[type];
        _currentState.OnEnter();
        Debug.Log($"Changing state to " + _currentState);
    }
    
    [Route]
    public void On(ChangeStateCommand command)
    {
        ChangeState(command.StateType);
    }
    
    public void Tick()
    {
        _currentState?.OnUpdate();
    }
    public void FixedTick() => _currentState?.OnFixedUpdate();
    public void LateTick() => _currentState?.OnLateUpdate();
}
public enum GameStateType
{
    Unknown = 0,
    Home = 1,
    InGame = 2,
    Win = 3,
    Lose = 4,
}