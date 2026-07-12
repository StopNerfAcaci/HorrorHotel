using System;
using Cysharp.Threading.Tasks;
using Gameplay.Inventory;
using MVP;
using R3;
using TMPro;
using UnityEngine;

namespace GameCore.Presentation
{
    public class PreviewPresenter: ModalPresenter<PreviewModal, PreviewViewState>
    {
        public ReactiveCommand<ItemSO> ItemPreviewCommand { get; } = new();
        private DisposableBag _bag;
        public PreviewPresenter(PreviewModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, PreviewViewState state, PreviewModal view)
        {
            _bag.Dispose();
            _bag = new DisposableBag();
            ItemPreviewCommand.Subscribe(item =>
            {
                view.UpdateText(item.name, item.description);
            }).AddTo(ref _bag);
            return UniTask.CompletedTask;
        }
    }
}