using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using Managers;
using R3;
using VitalRouter;

namespace Managers.FSM
{
    public class LobbyState : GameState
    {
        private readonly GameManager _gameManager;
        private readonly TransitionService _transitionService;
        private readonly ICommandPublisher _publisher;
        private readonly ICommandSubscribable _commandSubscribable;

        private DisposableBag _bag;

        public LobbyState(
            GameManager gameManager,
            TransitionService transitionService,
            ICommandPublisher publisher,
            ICommandSubscribable commandSubscribable)
        {
            _gameManager = gameManager;
            _transitionService = transitionService;
            _publisher = publisher;
            _commandSubscribable = commandSubscribable;
        }

        public override void OnEnter()
        {
            ShowLobbyScreen().Forget();
        }

        public override void OnExit()
        {
            //_gameManager.LobbyCamera.SetActive(false);
        }

        private async UniTask ShowLobbyScreen()
        {
            // var lobbyPresenter = await _transitionService.ShowLobbyScreen();

            // TODO: Check sound
            // await lobbyPresenter.StartLevel.ToNextValueUniTask();
            await _transitionService.ShowLoadingSubActivity(true);
            await UniTask.Delay(500);

            await _publisher.PublishAsync(new ChangeGameStateCommand(StateType.InGame));
            // Do NOT CloseLoadingSubActivity here. IngameState closes loading only after LoadLevel + InitPresenter
            // so Addressables cold-start cannot dismiss the overlay while CurrentLevelController is still null
            // (avoids empty gameplay UI + OnDoneLoading race).
        }
    }
}
