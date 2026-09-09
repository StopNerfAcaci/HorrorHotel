using System.Collections.Generic;
using Horror.Events;

public class GameStateMachine
{
    private readonly Dictionary<GameStateType, GameState> _states;
    private GameState _currentState;

    public GameStateMachine(GameplayManager manager)
    {
        HomeState home = new HomeState(manager);
        IngameState inGame = new IngameState(manager, manager.Player);
        WinState win = new WinState();
        LoseState lose = new LoseState();
        _states = new()
        {
            [GameStateType.Home] = home,
            [GameStateType.InGame] = inGame,
            [GameStateType.Win] = win,
            [GameStateType.Lose] = lose
        };
        ChangeState(GameStateType.Home);
    }

    public GameStateType CurrentStateType { get; private set; }

    internal void ChangeState(GameStateType type)
    {
        if (_currentState == _states[type])
            return;
        _currentState?.OnExit();
        _currentState = _states[type];
        _currentState.OnEnter();
    }

    public void Tick()
    {
        _currentState?.OnUpdate();
    }

    public void FixedTick() => _currentState?.OnFixedUpdate();
    public void LateTick() => _currentState?.OnLateUpdate();

    public void Dispose()
    {
    }
}

public enum GameStateType
{
    Unknown = 0,
    Home = 1,
    InGame = 2,
    Win = 3,
    Lose = 4,
}