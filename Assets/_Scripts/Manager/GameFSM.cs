using System.Collections.Generic;
using Managers;
using UnityEngine;
using VContainer.Unity;
using VitalRouter;

namespace Managers.FSM
{
    [Routes]
    public partial class GameFSM : ITickable, IFixedTickable, ILateTickable
    {
        private readonly Dictionary<StateType, GameState> _states;
        private GameState _currentState;

        public GameState CurrentState => _currentState;

        public GameFSM(GameManager gameManager,HomeState home, LobbyState lobby, IngameState inGame, WinState win, LoseState lose)
        {
            _states = new()
            {
                [StateType.Home] = home,
                [StateType.Lobby] = lobby,
                [StateType.InGame] = inGame,
                [StateType.Win] = win,
                [StateType.Lose] = lose
            };

            gameManager.GameFSM = this;
            //ChangeState(StateType.Lobby);
        }

        [Route]
        public void On(ChangeGameStateCommand command)
        {
            ChangeState(command.StateType);
        }

        private void ChangeState(StateType type)
        {
            if (_currentState == _states[type])
                return;

            _currentState?.OnExit();
            _currentState = _states[type];
            _currentState.OnEnter();
        }

        //public void Tick() => _currentState?.OnUpdateFill();
        public void Tick()
        {
            //UnityEngine.Debug.Log("current state " + _states.FirstOrDefault(x => x.Value == _currentState));
            _currentState?.OnUpdate();
        }

        public void FixedTick() => _currentState?.OnFixedUpdate();
        public void LateTick() => _currentState?.OnLateUpdate();
    }

    public abstract class GameState
    {
        protected GameState()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnLateUpdate()
        {
        }
    }

    public enum StateType
    {
        Home = 0,
        Lobby = 1,
        InGame = 2,
        Win = 3,
        Lose = 4,
    }

    public struct ChangeGameStateCommand : ICommand
    {
        public StateType StateType { get; }

        public ChangeGameStateCommand(StateType stateType)
        {
            StateType = stateType;
        }
    }
}