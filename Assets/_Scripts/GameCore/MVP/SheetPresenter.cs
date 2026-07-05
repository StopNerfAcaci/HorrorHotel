using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ZBase.UnityScreenNavigator.Core.Sheets;

namespace MVP
{
    public class SheetPresenter<TSheet, TViewState> : Presenter<TSheet>
        where TSheet : Sheet<TViewState>
        where TViewState : ViewState, new()
    {
        public TSheet View { get; }

        private TViewState _state;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();


        public SheetPresenter(TSheet view) : base(view)
        {
            View = view;
        }

        protected sealed override void Initialize(TSheet view)
        {
            _state = new TViewState();
            _disposables.Add(_state);
            View.SetupState(_state);
            Initialize(_state, View);
        }

        protected sealed override void Dispose(TSheet view)
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            OnDispose(view);
        }

        protected virtual void OnDispose(TSheet view)
        {
        }

        protected virtual UniTask Initialize(TViewState state, TSheet view)
        {
            return UniTask.CompletedTask;
        }
    }
}