using System;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Activities;

namespace MVP
{
    public abstract class Activity<TViewState> : Activity, IState<TViewState> where TViewState : ViewState
    {
        private TViewState _state;

        public void SetupState(TViewState state)
        {
            _state = state;
        }

        public sealed override UniTask Initialize(Memory<object> args)
        {
            return InitializeState(_state, args);
        }

        public override void DidEnter(Memory<object> args)
        {
            WillPushEnterState(_state, args).Forget();
        }

        public abstract UniTask InitializeState(TViewState state, Memory<object> args);

        public UniTask WillPushEnterState(TViewState state, Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
       
    }
}