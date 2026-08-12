using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.MVP
{
    public interface IState<in TViewState> where TViewState : ViewState
    {
        void SetupState(TViewState state);
        UniTask InitializeState(TViewState state);
        UniTask WillPushEnterState(TViewState state);
    }

    public class Popup : UIView
    {
        private readonly List<Popup> views = new();

        public virtual UniTask Initialize()
        {
            return UniTask.CompletedTask;
        }

        internal async UniTask AfterLoadAsync(RectTransform parentTransform)
        {
            views.Add(this);

            Parent = parentTransform;
            RectTransform.FillParent(Parent);

            Alpha = 0.0f;

            var tasks = views.Select(x => x.Initialize());
            await WaitForAsync(tasks);
        }

    }
    [DisallowMultipleComponent]
    public abstract class PopupView<TViewState> : Popup, IState<TViewState> where TViewState : ViewState
    {
        private TViewState _viewState;
        public void SetupState(TViewState state)
        {
            _viewState = state;
        }

        public UniTask InitializeState()
        {
            return InitializeState(_viewState);
        }

        public UniTask WillPushEnterState()
        {
            return WillPushEnterState(_viewState);
        }
        public abstract UniTask InitializeState(TViewState state);

        public virtual UniTask WillPushEnterState(TViewState state)
        {
            return UniTask.CompletedTask;
        }
    }
}