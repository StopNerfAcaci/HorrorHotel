using Cysharp.Threading.Tasks;
using Gamecore.Presentation.Home;
using GameCore.Presentation.Shared;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using VitalRouter;

namespace Managers.FSM
{
    public class HomeState : GameState
    {
        private readonly TransitionService _transitionService;
        private readonly ICommandPublisher _publisher;
        private readonly ICommandSubscribable _commandSubscribable;
        private readonly HomeManager _manager;
        private DisposableBag _bag;

        public HomeState(
            HomeManager homeManager,
            TransitionService transitionService,
            ICommandPublisher publisher,
            ICommandSubscribable commandSubscribable)
        {
            _manager = homeManager;
            _transitionService = transitionService;
            _publisher = publisher;
            _commandSubscribable = commandSubscribable;
        }

        public override void OnEnter()
        {
            _bag.Dispose();
            _bag = new DisposableBag();
            ShowHomeScreen().Forget();
        }

        public override void OnExit()
        {
            _bag.Dispose();
        }

        private async UniTask ShowHomeScreen()
        {
            var homePresenter = await _transitionService.ShowHomeScreen();
            homePresenter.PlayCommand.SubscribeAwait(async (_, ct) =>
            {
                await _transitionService.ShowLoadingSubActivity(true);
                await UniTask.Delay(500, cancellationToken: ct);
                await SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
                _manager.ToggleMainCam(false);
                await _publisher.PublishAsync(new ChangeGameStateCommand(StateType.InGame));
            }).AddTo(ref _bag);
            
            homePresenter.SettingCommand.Subscribe(_ =>
            {
                // Show settings modal/screen here
                Debug.Log("Open settings");
            }).AddTo(ref _bag);

            homePresenter.CreditCommand.Subscribe(_ =>
            {
                // Show credits modal/screen here
                Debug.Log("Open credits");
            }).AddTo(ref _bag);
        }
    }
}
