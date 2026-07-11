using System;
using Cysharp.Threading.Tasks;
using Gamecore.Presentation.Home;
using Managers.FSM;
using UnityEngine;

//For change states show
namespace GameCore.Presentation.Shared
{
    public partial class TransitionService
    {
        public async UniTask<HomePresenter> ShowHomeScreen()
        {
            var presenter = await ShowScreenPresenterAsync<HomePresenter, HomeScreen, HomeViewState>(
                "HomeScreen",
                screen => new HomePresenter(screen), false, true);
            return presenter;
        }

        public async UniTask<PreviewPresenter> ShowPreviewModal()
        {
            var presenter = await ShowModalPresenterAsync<PreviewPresenter, PreviewModal, PreviewViewState>(
                "PreviewModal",modal =>  new PreviewPresenter(modal)
            );
            return presenter;
        }

    }
    
}
