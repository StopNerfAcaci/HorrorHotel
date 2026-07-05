using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Activities;

namespace MVP
{
    public class ActivityPresenter<TActivity, TViewState> : Presenter<TActivity>, IActivityLifecycleEvent
        where TActivity : Activity<TViewState>
        where TViewState : ViewState, new()
    {
        public TActivity View { get; }

        private TViewState _state;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public ActivityPresenter(TActivity view) : base(view)
        {
            View = view;
        }

        protected sealed override void Initialize(TActivity view)
        {
            view.AddLifecycleEvent(this, -100);
        }

        protected sealed override void Dispose(TActivity view)
        {
            view.RemoveLifecycleEvent(this);
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            Dispose(_state, View);
        }

        protected virtual void Dispose(TViewState state, TActivity view)
        {
        }


        UniTask IActivityLifecycleEvent.Initialize(Memory<object> args)
        {
            _state = new TViewState();
            _disposables.Add(_state);
            View.SetupState(_state);
            return Initialize(args, _state, View);
        }

        protected virtual UniTask Initialize(Memory<object> args, TViewState state, TActivity view)
        {
            return UniTask.CompletedTask;
        }

        void IActivityLifecycleEvent.DidEnter(Memory<object> args)
        {
            DidEnter(args, _state, View);
        }

        protected virtual void DidEnter(Memory<object> args, TViewState state, TActivity view)
        {
        }


        void IActivityLifecycleEvent.DidExit(Memory<object> args)
        {
            DidExit(args, _state, View);
        }

        protected virtual void DidExit(Memory<object> args, TViewState state, TActivity view)
        {
        }

        UniTask IActivityLifecycleEvent.WillEnter(Memory<object> args)
        {
            return WillEnter(args, _state, View);
        }

        protected virtual UniTask WillEnter(Memory<object> args, TViewState state, TActivity view)
        {
            return UniTask.CompletedTask;
        }


        UniTask IActivityLifecycleEvent.WillExit(Memory<object> args)
        {
            return WillExit(args, _state, View);
        }

        protected virtual UniTask WillExit(Memory<object> args, TViewState state, TActivity view)
        {
            return UniTask.CompletedTask;
        }


        UniTask IActivityLifecycleEvent.Cleanup(Memory<object> args)
        {
            return Cleanup(args, _state, View);
        }

        protected virtual UniTask Cleanup(Memory<object> args, TViewState state, TActivity view)
        {
            return UniTask.CompletedTask;
        }
    }
}