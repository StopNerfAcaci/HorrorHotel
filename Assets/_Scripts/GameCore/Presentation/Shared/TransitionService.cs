using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Loading;
using MVP;
using UnityEngine;
using VContainer;
using ZBase.UnityScreenNavigator.Core;
using ZBase.UnityScreenNavigator.Core.Activities;
using ZBase.UnityScreenNavigator.Core.Modals;
using ZBase.UnityScreenNavigator.Core.Screens;
using ZBase.UnityScreenNavigator.Core.Windows;

namespace GameCore.Presentation.Shared
{
    public interface ICloseTransition
    {
        public UniTask ClosePopup();
        UniTask ClosePopupForce();
    }

    public partial class TransitionService : ICloseTransition
    {
        private IObjectResolver _resolver;

        private ModalContainer _modalContainer;
        private ActivityContainer _activityContainer;
        private ActivityContainer _loadingContainer;
        private ScreenContainer _screenContainer;

        public bool IsModalInTransition => _modalContainer.IsInTransition;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public void FindContainer(IWindowContainerManager containerManager)
        {
            _modalContainer = containerManager.Find<ModalContainer>();
            _activityContainer = containerManager.Find<ActivityContainer>("ActivityContainer");
            _loadingContainer = containerManager.Find<ActivityContainer>("LoadingContainer");
            _screenContainer = containerManager.Find<ScreenContainer>();
        }

        public UniTask ClosePopup()
        {
            var modalsCount = _modalContainer.Modals.Count;
            return modalsCount > 0 ? _modalContainer.PopAsync(true) : UniTask.CompletedTask;
        }

        public UniTask ClosePopupForce()
        {
            var modalsCount = _modalContainer.Modals.Count;
            return modalsCount > 0 ? _modalContainer.PopAsync(false) : UniTask.CompletedTask;
        }

        public UniTask<bool> BackToPreviousScreen()
        {
            return _screenContainer.Screens.Count > 1
                ? _screenContainer.PopAsync(true)
                    .ContinueWith(() => true)
                : UniTask.FromResult(false);
        }

        private UniTask<T> ShowModalPresenterAsync<T, TView, TState>(string key, Func<TView, T> createFunc,
            bool isPlayAnimShowModal = true)
            where T : ModalPresenter<TView, TState>
            where TView : Modal<TState>
            where TState : ViewState, new()
        {
            var tcs = new UniTaskCompletionSource<T>();
            var options = new ModalOptions(key, onLoaded: (view, args) =>
            {
                var presenter = createFunc((TView)view);
                _resolver.Inject(presenter);
                presenter.Initialize();
                tcs.TrySetResult(presenter);
            }, playAnimation: isPlayAnimShowModal);
            _modalContainer.Push<TView>(options);
            return tcs.Task;
        }

        private async UniTask<T> ShowScreenPresenterAsync<T, TView, TState>(string key, Func<TView, T> createFunc,
            bool isStack = true, bool isPooling = false)
            where T : ScreenPresenter<TView, TState>
            where TView : Screen<TState>
            where TState : ViewState, new()
        {
            T presenter = null;
            var options = new ScreenOptions(key,
                onLoaded: (view, args) =>
                {
                    presenter = createFunc((TView)view);
                    _resolver.Inject(presenter);
                    presenter.Initialize();
                }, stack: isStack,
                poolingPolicy: isPooling ? PoolingPolicy.EnablePooling : PoolingPolicy.DisablePooling);

            await _screenContainer.PushAsync<TView>(options);
            return presenter;
        }

        private UniTask<T> ShowActivityPresenterAsync<T, TView, TState>(
            ActivityContainer container,
            string key,
            Func<TView, T> createFunc)
            where T : ActivityPresenter<TView, TState>
            where TView : Activity<TState>
            where TState : ViewState, new()
        {
            var tcs = new UniTaskCompletionSource<T>();
            var options = new ActivityOptions(key, onLoaded: (view, args) =>
            {
                var presenter = createFunc((TView)view);
                _resolver.Inject(presenter);
                presenter.Initialize();
                tcs.TrySetResult(presenter);
            });
            container.Show<TView>(options);
            return tcs.Task;
        }

        public async UniTask<LoadingSubPresenter> ShowLoadingSubActivity(bool isShow = true)
        {
            if (isShow)
            {
                var presenter =
                    await ShowActivityPresenterAsync<LoadingSubPresenter, LoadingActivity, LoadingViewState>(
                        _loadingContainer,
                        "LoadingScreen",
                        activity => new LoadingSubPresenter(activity));
                return presenter;
            }
            else
            {
                if (_loadingContainer.Activities.TryGetValue("LoadingScreen", out var activity))
                {
                    await _loadingContainer.HideAsync("LoadingScreen");
                    return null;
                }

                return null;
            }
        }

        public async UniTask<InventoryPresenter> ShowInventoryUI()
        {
            var presenter = await ShowScreenPresenterAsync<InventoryPresenter, InventoryScreen, InventoryViewState>(
                "InventoryScreen",
                screen => new InventoryPresenter(screen), false, true);
            
            return presenter;
        }

        public async UniTask<GameplayPresenter> ShowGamePlayScreen()
        {
            // isPooling = true: keep GamePlayScreen GameObject alive across level transitions so its
            // serialized sprite/material references (bg_bot, MaskWriterAlpha, ...) stay valid. Without
            // pooling the Addressables release of the outgoing instance races with the incoming one and
            // Unity nulls shared references on some Android devices (reproduced in build).
            var presenter = await ShowScreenPresenterAsync<GameplayPresenter, GameplayScreen, GameplayViewState>(
                "GameplayScreen",
                screen => new GameplayPresenter(screen), false, true);
            return presenter;
        }

        // public async UniTask<LobbyPresenter> ShowLobbyScreen()
        // {
        //     var presenter = await ShowScreenPresenterAsync<LobbyPresenter, LobbyScreen, LobbyViewState>(
        //         "LobbyScreen",
        //         screen => new LobbyPresenter(screen), false);
        //     return presenter;
        // }
    }
}