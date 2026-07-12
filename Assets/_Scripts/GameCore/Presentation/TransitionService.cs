using System;
using Cysharp.Threading.Tasks;

//For change states show
namespace GameCore.Presentation.Shared
{
    public partial class TransitionService
    {
        public async UniTask<PreviewPresenter> ShowPreviewModal()
        {
            var presenter = await ShowModalPresenterAsync<PreviewPresenter, PreviewModal, PreviewViewState>(
                "PreviewModal",modal =>  new PreviewPresenter(modal)
            );
            return presenter;
        }

    }
    
}
