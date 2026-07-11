using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Modals;

namespace MVP
{
    public abstract class ModalPresenter<TModal, TViewState> : Presenter<TModal>, IModalLifecycleEvent
        where TModal : Modal<TViewState>
        where TViewState : new()
    {
        public TModal View { get; }

        private TViewState _state;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public ModalPresenter(TModal view) : base(view)
        {
            View = view;
        }

        protected sealed override void Initialize(TModal view)
        {
            view.AddLifecycleEvent(this, -100);
        }

        protected sealed override void Dispose(TModal view)
        {
            view.RemoveLifecycleEvent(this);
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            Dispose(_state, View);
        }
        
        protected virtual void Dispose(TViewState state, TModal view)
        { }


        UniTask IModalLifecycleEvent.Initialize(Memory<object> args)
        {
            _state = new TViewState();
            _disposables.Add(_state);
            View.SetupState(_state);
            return Initialize(args, _state, View);
        }

        protected abstract UniTask Initialize(Memory<object> args, TViewState state, TModal view);

        UniTask IModalLifecycleEvent.WillPushEnter(Memory<object> args)
        {
            return WillPushEnter(args, _state, View);
        }

        protected virtual UniTask WillPushEnter(Memory<object> args, TViewState state, TModal view)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPushEnter(Memory<object> args)
        {
            DidPushEnter(args, _state, View);
        }

        protected virtual void DidPushEnter(Memory<object> args, TViewState state, TModal view)
        {
        }

        UniTask IModalLifecycleEvent.WillPushExit(Memory<object> args)
        {
            return WillPushExit(args, _state, View);
        }

        protected virtual UniTask WillPushExit(Memory<object> args, TViewState state, TModal view)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPushExit(Memory<object> args)
        {
            DidPushExit(args, _state, View);
        }

        protected virtual void DidPushExit(Memory<object> args, TViewState state, TModal view)
        {
        }

        UniTask IModalLifecycleEvent.WillPopEnter(Memory<object> args)
        {
            return WillPopEnter(args, _state, View);
        }

        protected virtual UniTask WillPopEnter(Memory<object> args, TViewState state, TModal view)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPopEnter(Memory<object> args)
        {
            DidPopEnter(args, _state, View);
        }

        protected virtual UniTask DidPopEnter(Memory<object> args, TViewState state, TModal view)
        {
            return UniTask.CompletedTask;
        }

        UniTask IModalLifecycleEvent.WillPopExit(Memory<object> args)
        {
            return WillPopExit(args, _state, View);
        }

        protected virtual UniTask WillPopExit(Memory<object> args, TViewState state, TModal view)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPopExit(Memory<object> args)
        {
            DidPopExit(args, _state, View);
        }

        protected virtual void DidPopExit(Memory<object> args, TViewState state, TModal view)
        {
        }

        UniTask IModalLifecycleEvent.Cleanup(Memory<object> args)
        {
            return Cleanup(args, _state, View);
        }

        protected virtual UniTask Cleanup(Memory<object> args, TViewState state, TModal view)
        {
            return UniTask.CompletedTask;
        }
    }
}