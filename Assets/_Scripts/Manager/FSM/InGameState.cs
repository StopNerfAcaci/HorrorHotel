using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCore.Presentation;
using GameCore.Presentation.Loading;
using GameCore.Presentation.Shared;
using R3;
using UnityEngine;
using VitalRouter;

namespace Managers.FSM
{
    public class StartLevelCommand : ICommand
    {
        public int Level;
        public int StarCount;
    }

    public class IngameState
    {
        private GameManager _gameManager;
        private readonly ICommandPublisher _publisher;

        private readonly TransitionService _transitionService;

        // private readonly SettingUseCase _settingUseCase;
        private readonly ICommandSubscribable _commandSubscribable;
        private GameplayPresenter _gamePlayPresenter;
        private DisposableBag _bag;
        // private bool isPlay = false;

        public IngameState(GameManager gameManager,
            ICommandPublisher publisher,
            ICommandSubscribable commandSubscribable,
            TransitionService transitionService 
        )
        {
            _gameManager = gameManager;
            _publisher = publisher;
            _commandSubscribable = commandSubscribable;
            _transitionService = transitionService;
        }

        public void OnEnter()
        {
            OnEnterAsync().Forget();
        }

        async UniTask OnEnterAsync()
        {   
            await SetupPresenter();
            try
            {
                await _publisher.PublishAsync(new StartLevelCommand() { });

                // Init presenter (bounds, level type, timer, hint) BEFORE closing loading
                // so the screen is fully synced when loading disappears — avoids "reset" feel.
                _gameManager.InitPresenter(_gamePlayPresenter);
                await UniTask.Delay(500);
                await _publisher.PublishAsync(new CloseLoadingSubActivityCommand());
                // Close is only from here (not from Lobby) so OnDoneLoading runs after level exists.
                // Explicit call still ensures start flow if anything drops the event.
                _gameManager.OnLoadingSubDone();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exception in load branch: {e}");
                await _publisher.PublishAsync(new CloseLoadingSubActivityCommand());
            }

            StartListening();
        }

        private void CheckStartGame()
        {
            _gamePlayPresenter.StartLevel().Forget();
        }
        public void OnExit()
        {
            Debug.Log($"CHECK LOSE: EXIT INGAME STATE");
            StopListening();
        }

        private async UniTask SetupPresenter()
        {
            _gamePlayPresenter = await _transitionService.ShowGamePlayScreen();
            
            _gamePlayPresenter.QuitLevel.Subscribe(_ =>
                _gameManager.QuitLevel()
            ).AddTo(ref _bag);

            _gamePlayPresenter.PauseCommand.Subscribe(isPause =>
                _gameManager.PauseGame(isPause));
            _commandSubscribable.SubscribeAwait<OnDoneLoadingCommand>(OnDoneLoading).AddTo(ref _bag);
        }

        private ValueTask OnDoneLoading(OnDoneLoadingCommand command, PublishContext ctx)
        {
            _gameManager?.OnLoadingSubDone();
            return default;
        }

        // private async UniTask PlayMusic()
        // {
        //     var audio = AudioManager.PlayMusic(MusicType.Home /*Music Gameplay*/);
        //     if (!AudioManager.MusicMuted)
        //     {
        //         audio.AudioSource.volume = 0;
        //         audio.AudioSource.DOFade(1, 0.5f);
        //     }
        // }

        private void StartListening()
        {
            Debug.Log($"INGAME ENTER ");

        }

        private void StopListening()
        {
            Debug.Log($"INGAME EXIT ");
        }
    }
}