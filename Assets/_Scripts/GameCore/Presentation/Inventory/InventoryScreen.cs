using System;
using Cysharp.Threading.Tasks;
using MVP;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class InventoryScreen : Screen<InventoryViewState>
{
    [SerializeField] private Button closeBtn;
    
    private DisposableBag _bag;
    public override UniTask InitializeState(InventoryViewState state, Memory<object> args)
    {
        closeBtn.SubscribeToCommand(state.ClosePopupCommand)
            .AddTo(ref _bag);
        return UniTask.CompletedTask;
    }
}