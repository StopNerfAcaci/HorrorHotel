using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MVP
{
    public abstract class AppViewPresenter<TAppView, TViewState> : Presenter<TAppView>
        where TAppView : AppView<TViewState>
        where TViewState : ViewState, new()
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private TViewState _state;
        public TAppView View { get; }

        public AppViewPresenter(TAppView view) : base(view)
        {
            View = view;
        }

        protected sealed override void Initialize(TAppView view)
        {
            _state = new TViewState();
            _disposables.Add(_state);
            Initialize(_state, View).ContinueWith(() => view.InitializeAsync(_state));
        }

        protected sealed override void Dispose(TAppView view)
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            Dispose(_state, View);
        }

        protected abstract UniTask Initialize(TViewState state, TAppView view);

        protected virtual void Dispose(TViewState state, TAppView view)
        {
        }
    }
}