using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Screens;

namespace MVP
{
    public class ScreenPresenter<TScreen, TViewState> : Presenter<TScreen>, IScreenLifecycleEvent
        where TScreen : Screen<TViewState>
        where TViewState : ViewState, new()
    {
        public TScreen View { get; }

        private TViewState _state;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public ScreenPresenter(TScreen view) : base(view)
        {
            View = view;
        }

        protected sealed override void Initialize(TScreen view)
        {
            view.AddLifecycleEvent(this, -100);
        }

        protected sealed override void Dispose(TScreen view)
        {
            view.RemoveLifecycleEvent(this);
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            Dispose(_state, View);
        }

        protected virtual void Dispose(TViewState state, TScreen view)
        {
        }
        
        UniTask IScreenLifecycleEvent.Initialize(Memory<object> args)
        {
            _state = new TViewState();
            _disposables.Add(_state);
            View.SetupState(_state);
            return Initialize(args, _state, View);
        }

        protected virtual UniTask Initialize(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }

        UniTask IScreenLifecycleEvent.WillPushEnter(Memory<object> args)
        {
            return WillPushEnter(args, _state, View);
        }

        protected virtual UniTask WillPushEnter(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPushEnter(Memory<object> args)
        {
            DidPushEnter(args, _state, View);
        }

        protected virtual void DidPushEnter(Memory<object> args, TViewState state, TScreen view)
        {
        }

        UniTask IScreenLifecycleEvent.WillPushExit(Memory<object> args)
        {
            return WillPushExit(args, _state, View);
        }

        protected virtual UniTask WillPushExit(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPushExit(Memory<object> args)
        {
            DidPushExit(args, _state, View);
        }

        protected virtual void DidPushExit(Memory<object> args, TViewState state, TScreen view)
        {
        }

        UniTask IScreenLifecycleEvent.WillPopEnter(Memory<object> args)
        {
            return WillPopEnter(args, _state, View);
        }

        protected virtual UniTask WillPopEnter(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPopEnter(Memory<object> args)
        {
            DidPopEnter(args, _state, View);
        }

        protected virtual UniTask DidPopEnter(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }

        UniTask IScreenLifecycleEvent.WillPopExit(Memory<object> args)
        {
            return WillPopExit(args, _state, View);
        }

        protected virtual UniTask WillPopExit(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPopExit(Memory<object> args)
        {
            DidPopExit(args, _state, View);
        }

        protected virtual void DidPopExit(Memory<object> args, TViewState state, TScreen view)
        {
        }

        UniTask IScreenLifecycleEvent.Cleanup(Memory<object> args)
        {
            return Cleanup(args, _state, View);
        }

        protected virtual UniTask Cleanup(Memory<object> args, TViewState state, TScreen view)
        {
            return UniTask.CompletedTask;
        }
    }
}