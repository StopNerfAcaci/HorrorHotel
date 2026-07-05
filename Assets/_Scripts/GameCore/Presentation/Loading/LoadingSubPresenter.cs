using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using MVP;
using R3;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace GameCore.Presentation.Loading
{
    public struct CloseLoadingSubActivityCommand : ICommand
    {
    }
    public struct OnDoneLoadingCommand : ICommand
    {
    }

    public class LoadingSubPresenter : ActivityPresenter<LoadingActivity, LoadingViewState>
    {
        private TransitionService _transitionService;
        private ICommandSubscribable _commandSubscribable;
        private LoadingViewState _state;
        private LoadingActivity _view;
        private ICommandPublisher _publisher;
        private DisposableBag _bag;
        
        [Inject]
        private void Construct(TransitionService transitionService, ICommandPublisher publisher, ICommandSubscribable commandSubscribable)
        {
            _transitionService = transitionService;
            _publisher = publisher;
            _commandSubscribable = commandSubscribable;
        }

        public LoadingSubPresenter(LoadingActivity view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, LoadingViewState state, LoadingActivity view)
        {
            Debug.Log($"LOADING SUB INIT");
            _state = state;
            _view = view;
            _commandSubscribable.SubscribeAwait<CloseLoadingSubActivityCommand>(OnCloseLoadingSubActivity).AddTo(view);
            return UniTask.CompletedTask;
        }
        private async ValueTask OnCloseLoadingSubActivity(CloseLoadingSubActivityCommand command, PublishContext ctx)
        {
            // Close command means level load done.
            // If fill tween has not completed yet, wait here.
            // If fill tween completed earlier, this continues immediately.
           
            await UniTask.WaitUntil(() => _state.IntroAnimationComplete);
            await _view.PlayMoveOut();
           
            await _transitionService.ShowLoadingSubActivity(false);
            await _publisher.PublishAsync(new OnDoneLoadingCommand());
        }
        public override void Dispose()
        {
            _bag.Dispose();
        }
    }
}