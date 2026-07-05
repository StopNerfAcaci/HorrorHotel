using System;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Sheets;

namespace MVP
{
    public abstract class Sheet<TViewState> : Sheet, IState<TViewState> where TViewState : ViewState
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

        public override UniTask WillEnter(Memory<object> args)
        {
            return WillPushEnterState(_state, args);
        }

        public abstract UniTask InitializeState(TViewState state, Memory<object> args);

        public virtual UniTask WillPushEnterState(TViewState state, Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}