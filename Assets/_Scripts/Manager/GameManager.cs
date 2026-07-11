using System;
using Cysharp.Threading.Tasks;
using GameCore.Presentation;
using GameCore.Presentation.Shared;
using HSM;
using Managers.FSM;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Managers
{
    //Only manage finite machine state
    public sealed class GameManager : MonoBehaviour
    {
        public event Action ActionStartGame;

        public StateMachine GameSM { get; set; }
        private GameplayPresenter _gamePlayPresenter;

        private bool _hasTriggeredStartGameFlow;
        private bool _allowGameplayTimer;
        private bool hasStarted;

        private State gameRoot;
        public bool IsGameplayTimerAllowed => _allowGameplayTimer;

        private void Awake()
        {
            gameRoot = new GameRoot(null, this);
            var builder = new StateMachineBuilder(gameRoot);
            GameSM = builder.Build();
        }

        private void Start()
        {
            _allowGameplayTimer = false;
        }

        public void InitPresenter(GameplayPresenter gamePlayPresenter)
        {
            _gamePlayPresenter = gamePlayPresenter;
        }

        public void OnLoadingSubDone()
        {
            if (_hasTriggeredStartGameFlow)
            {
                return;
            }

            _hasTriggeredStartGameFlow = true;
            HandleStartGameFlowAsync().Forget();
            // PlayThemeMusic();
        }

        private async UniTask HandleStartGameFlowAsync()
        {
            await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
            TriggerActionStartGame();
        }

        private void TriggerActionStartGame()
        {
            _allowGameplayTimer = true;
            ActionStartGame?.Invoke();
            // ShowTutorialCommand?.Execute(default);
            CompleteGameplayStartAfterEnterFlow();
        }

        private void CompleteGameplayStartAfterEnterFlow()
        {
            if (hasStarted)
                return;

            hasStarted = true;
            if (_gamePlayPresenter == null)
                return;
            _gamePlayPresenter.StartLevel().Forget();
        }

        public void QuitLevel()
        {
            //TODO: quit logic
        }

        public void PauseGame(bool isPause)
        {
            //TODO: pause logic
        }
    }
}