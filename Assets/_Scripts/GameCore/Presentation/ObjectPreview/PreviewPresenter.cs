using System;
using Cysharp.Threading.Tasks;
using MVP;
using R3;

namespace GameCore.Presentation
{
    public class PreviewPresenter: ModalPresenter<PreviewModal, PreviewViewState>
    {
        private DisposableBag _bag;
        public PreviewPresenter(PreviewModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, PreviewViewState state, PreviewModal view)
        {
            _bag.Dispose();
            _bag = new DisposableBag();
            return UniTask.CompletedTask;
        }
    }
}