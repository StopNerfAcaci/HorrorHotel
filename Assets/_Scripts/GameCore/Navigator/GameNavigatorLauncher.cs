using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using Managers.FSM;
using VContainer;
using VitalRouter;
using ZBase.UnityScreenNavigator.Core;

namespace GameCore.Navigator
{
    public class GameNavigatorLauncher : UnityScreenNavigatorLauncher
    {
        [Inject] private TransitionService _transitionService;
        [Inject] private ICommandPublisher _publisher;
       

        protected override void OnPostCreateContainers()
        {
            UnityScreenNavigatorSettings.Initialize();
            _transitionService.FindContainer(this);
            StartFSM().Forget();
            UnityEngine.Debug.Log("On done create containers");
        }

        async UniTask StartFSM()
        {
            await _publisher.PublishAsync(new ChangeGameStateCommand(StateType.Home));

            //_publisher.PublishAsync(new ChangeGameStateCommand(StateType.InGame)).AsUniTask().Forget();
        }

        public TransitionService TransitionService => _transitionService;
    }
}
