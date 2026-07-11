using System;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Modals;

namespace MVP
{
    public abstract class Modal<TViewState> : Modal, IState<TViewState>
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

        public override UniTask WillPushEnter(Memory<object> args)
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